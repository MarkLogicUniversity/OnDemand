xquery version "1.0-ml";

(: alerting app tutorial action :)
(: from MarkLogic University, http://mlu.marklogic.com :)

declare namespace alert = "http://marklogic.com/xdmp/alert";
declare namespace em = "URN:ietf:params:email-xml:";

import module "http://marklogic.com/xdmp/alert" at "/MarkLogic/alert.xqy";

declare variable $alert:config-uri as xs:string external;
declare variable $alert:doc as node() external;
declare variable $alert:rule as element(alert:rule) external;
declare variable $alert:action as element(alert:action) external;

declare function local:send-email($rule as element(alert:rule), $doc as node())
as empty-sequence()
{
for $contact in $rule//contact
let $name := $contact//name
let $email := $contact//email
return
let $quote := "&#34;"
let $message :=
    <em:Message
     xmlns:em="URN:ietf:params:email-xml:"
     xmlns:rf="URN:ietf:params:rfc822:">
      <rf:subject>New book arrival!</rf:subject>
      <rf:from>
        <em:Address>
          <em:name>Alert System</em:name>
          <em:adrs>alert-demo@marklogic.com</em:adrs>
        </em:Address>
      </rf:from>
      <rf:to>
        <em:Address>
          <em:name>{$name}</em:name>
          <em:adrs>{$email}</em:adrs>
        </em:Address>
      </rf:to>
      <em:content xml:space="preserve">
      You expressed interest in new books meeting your interests! A new book
      titled {fn:concat($quote,$doc//title/string(),$quote)} by {$doc//author/string()} just arrived.
      </em:content>
    </em:Message>
return
(
  xdmp:log(fn:concat("Alerting App Tutorial Email sample: ", xdmp:quote($message)))
)
};

local:send-email($alert:rule, $alert:doc)

(:
,
xdmp:log(
  fn:concat(
    "alert-tutorial-email debug(",
    $alert:config-uri,
    ", ",
    xdmp:quote($alert:doc),
    ", ",
    xdmp:quote($alert:rule),
    ", ",
    xdmp:quote($alert:action),
    " rule-node: ",
    xdmp:quote($alert:rule),
    ")"
  )
)

:)
