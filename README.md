# SchoolLibrary
School Library - RESTful API using ASP.NET Core Web API

This RESTful API exposes some of the services (without any authentication/authorization) which are as below

Add New Book: api/Books (POST)
Get All Books: api/Books (GET)
Add New Student: api/Student (POST)
Get Student: api/Student/{id} (GET)
Assign Book: api/Books/Assign (POST)
Extend Return: api/Books/Extend (PUT)
Overdue Books: api/Books/Overdue (GET)

There system uses In-Memory Data Storage for the time being therefore data will not persist after service is closed or restarted.

Currently, there is not logging happening but can be added later upon need.

The project also includes test project which cover testing of services, controller and API calls.
