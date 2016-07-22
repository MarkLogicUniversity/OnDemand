xquery version "1.0-ml";

declare variable $form-success := xdmp:get-request-field("submit", "0");

xdmp:set-response-content-type("text/html; charset=utf-8"),
'<!DOCTYPE html>',
<html lang="en">
  <head>
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>MarkLogic University Sample Bookstore</title>

    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet"/>

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
  </head>
  <body>
  <div class="container-fluid">
    <!-- begin title header -->
    <div class="page-header">
      <div class="row">
        <div class="col-xs-2">
        &nbsp;
        </div>
        <div class="col-xs-1">
          <img src="img/book-20clip-20art-Book4.jpg" height="80" width="60"/>
        </div>
        <div class="col-xs-7">
          <h1>MarkLogic University Sample Bookstore</h1>
        </div>
        <div class="col-xs-2">
        &nbsp;
        </div>
      </div>
    </div>

    <div class="row"> <!-- begin body -->
      <div class="col-xs-2">
      &nbsp;
      </div>
      <div class="col-xs-8">
        <p>Books currently in stock</p>
        <div class="table-responsive">
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Title</th>
              <th>Author</th>
              <th>Price</th>
            </tr>
          </thead>
          <tbody>
          {
            for $book in //book
            order by $book/title ascending
            return
              <tr>
                <td>{$book/title/string()}</td>
                <td>{fn:string-join($book/author, ", ")}</td>
                <td style="text-align:right">{$book/price/string()}</td>
              </tr>
          }
          </tbody>
        </table>
        </div> <!-- end of table-responsive -->
      </div>
      <div class="col-xs-2">
      &nbsp;
      </div>
    </div> <!-- end of body -->
<hr/>
    <div class="row"> <!-- begin alert me form -->
      <div class="col-xs-2">
      &nbsp;
      </div>
      {
        if ($form-success eq "1")
        then
        <div class="col-xs-8">
          <p>We have your email and will let you know when books that interest you arrive.</p>
          <form role="form" class="form-horizontal" action="default.xqy" method="post">
            <button type="submit" class="btn btn-default">Thank you!</button>
          </form>
        </div>
        else
        <div class="col-xs-8">
          <p>Want us to email you when a book arrives?</p>
          <form role="form" class="form-horizontal" action="form-action.xqy" method="post" enctype="multipart/form-data">
            <div class="form-group">
              <label class="control-label col-xs-2" for="title">Book title:</label>
              <div class="col-xs-10">
                <input type="text" class="form-control" name="title" id="title" placeholder="Enter a title"/>
              </div>
            </div>
            <div class="form-group">
              <label class="control-label col-xs-2" for="author">Author:</label>
              <div class="col-xs-10">
                <input type="text" class="form-control" name="author" id="author" placeholder="Enter an author"/>
              </div>
            </div>
            <div class="form-group">
              <label class="control-label col-xs-2" for="name">Name:</label>
              <div class="col-xs-10">
                <input type="text" class="form-control" name="name" id="name" placeholder="Enter your name"/>
              </div>
            </div>
            <div class="form-group">
              <label class="control-label col-xs-2" for="email">Email address:</label>
              <div class="col-xs-10">
                <input type="email" class="form-control" name="email" id="email" placeholder="Enter your email address"/>
              </div>
            </div>
            <button type="submit" class="btn btn-default">Submit</button>
          </form>
        </div>
      }
      <div class="col-xs-2">
      &nbsp;
      </div>
    </div> <!-- end of alert me form -->

  </div> <!--end of container -->

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/bootstrap.min.js"></script>
  </body>
</html>
