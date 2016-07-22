xquery version "1.0-ml";

import module namespace alert = "http://marklogic.com/xdmp/alert"
          at "/MarkLogic/alert.xqy";

declare variable $title := xdmp:get-request-field("title");
declare variable $author := xdmp:get-request-field("author");
declare variable $name := xdmp:get-request-field("name");
declare variable $email := xdmp:get-request-field("email");

(: if we have an email address and at least one other field then
    that counts as good data for this sample app. Normally, one
    would check for valid email address formats, at the least.
:)
declare function local:have-good-data() as xs:boolean
{
  if (fn:string-length($email) eq 0)
  then fn:false()
  else
    if (fn:string-length($title) gt 0)
    then fn:true()
    else
      if (fn:string-length($author) gt 0)
      then fn:true()
      else fn:false()
};

declare function local:get-interest-doc()
{
  if (fn:doc-available("/query/book-interest.xml"))
  then fn:doc("/query/book-interest.xml")
  else
    xdmp:document-insert("/query/book-interest.xml",
      <book-interests>
        <contacts>
          <contact>
            <interests>
              <interest>
                <title>{$title}</title>
                <author>{$author}</author>
              </interest>
            </interests>
            <name>{$name}</name>
            <email>{$email}</email>
          </contact>
        </contacts>
      </book-interests>
    )
};

declare function local:add-to-interest-doc()
{
  let $doc := local:get-interest-doc()
  return if (fn:empty($doc))
         then local:add-rule()
         else
          let $_tmp := xdmp:node-insert-child($doc//interests,
                         <interest>
                           <title>{$title}</title>
                           <author>{$author}</author>
                         </interest>
                        )
          let $_tmp := xdmp:node-insert-child($doc//contacts,
                         <contact>
                          <name>{$name}</name>
                          <email>{$email}</email>
                         </contact>
                        )
          return local:add-rule()
};

declare function local:add-rule()
{
  let $config-uri := "http://marklogic.com/mlu/alert/app/sample/uri"
  let $rule-name := fn:concat("Book-Interest-", $name)
  let $rule-description := fn:concat("Customer interested in ", $title, " or ", $author)
  let $rule-user := 0
  let $rule-action := "Email-Updates"
  let $rule-options := <alert:options>
                        <contact>
                          <name>{$name}</name>
                          <email>{$email}</email>
                        </contact>
                       </alert:options>
  return
       alert:rule-insert($config-uri,
        alert:make-rule(
          $rule-name,
          $rule-description,
          $rule-user,
          cts:or-query((cts:element-word-query(xs:QName("title"), $title),
                        cts:element-word-query(xs:QName("author"), $author))),
          $rule-action,
          $rule-options
         )
        )
};

let $log-data := fn:concat("MLU Bookstore form data: title=",$title," author=", $author, " email=",$email)
let $_tmp := xdmp:log($log-data, "info")
let $_tmp := if (local:have-good-data() eq fn:true())
             then local:add-to-interest-doc()
             else ()
return xdmp:redirect-response("default.xqy?submit=1")
