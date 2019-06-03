<!doctype html>
<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HBR_Port_GUI._Default" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.IO" %>

<html lang="en">
<head runat="server">

    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
</head>

<body>

    <!-- Code goes in here to design page -->

    <form method="get" action="Default.aspx">
        <select name="rank">
            <option value="fm">financial mgr</option>
            <option value="director">director</option>
        </select>
        <input type="submit" value="Submit" />
    </form>

        <div class="container">
            <div class="dropdown">
                <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Dropdown button</button>
                <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                    <a class="dropdown-item" href="#">Director</a>
                    <a class="dropdown-item" href="#">Financial Manager</a>
                </div>
            </div>
            <br />
            <form>
                <table class="table">
                    <thead class="thead-dark">
                        <tr>
                            <th scope="col">Approve</th>
                            <th scope="col">Deny</th>
                            <th scope="col">Request_ID</th>
                            <th scope="col">Estimated Cost</th>
                            <th scope="col">Employee ID</th>
                            <th scope="col">Beg Date</th>
                            <th scope="col">End Date</th>
                            <th scope="col">Decision Date</th>
                            <th scope="col">Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Literal ID="Somename" runat="server" />
                    </tbody>
                </table>
                <button type="button" class="btn btn-primary btn-lg btn-block">Block level button</button>
                </form>
            <br\>
            <br\>
        </div>
    <br\>

    <!-- Code goes in here to design page -->
    <!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
</body>
</html>
