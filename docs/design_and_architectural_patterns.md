# Design and architectural patterns

## Value object

ISBN class is a value object. It is immutable and its equality is based on the value of its attributes. 

## Repository

There are a few repositories in the project. They are used to store and retrieve data from the database. Like BookRepository, UserRepository, etc.

## Service layer

The service layer is responsible for the business logic of the application. It is used to implement the use cases of the application. Like BookService, UserService, etc.

## Mapper

The mapper is used to convert objects from one type to another. Like BookMapper.

## Model View Controller (MVC)

The project is structured using the MVC pattern. The model is the domain model, the view is the user interface, and the controller is the application logic.

## Data Transfer Object (DTO)

The project uses DTOs to transfer data between the layers of the application. Like BookDTO, UserDTO, etc.

## Client Session State

The project uses the client session state pattern to store the state of the user session on the client side.

## Object-Relational Mapping (ORM) - Entity Framework Core

The project uses Entity Framework Core as an ORM to map the domain model to the database.

