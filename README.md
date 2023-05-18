# CodeGenerator
Code generator with optional transformation rules

## Project specification
The purpose of this project is to design and implement a tool for generating object code from class diagrams, creating an interface, and setting up transformation rules for converting the diagram into several languages. The tool will allow the user to add transformation rules to any language for generation purposes.

## Deployment

To deploy the system, you will need Docker installed on your machine. You can simply run `docker-compose up` or `docker-compose up --build` (to initiate build) to run the entire system at the root directory of your project.

- **Client application URL:** `http://localhost/Client` (port 80)
- **Server URL:** `localhost:5000`
- **Swagger Documentation:** `http://localhost:5000/swagger/index.html`

Using Docker-compose, you can easily manage the deployment of your project and ensure that all the necessary components are running smoothly together. This allows you to conveniently access the client application, server, and API documentation in your local environment for testing and development purposes.

## Docs
* Project documentation [CZ]: [/doc/SemestralProject_CodeGen_kon0379.pdf](./doc/SemestralProject_CodeGen_kon0379.pdf)
* Class diagram [SVG]: [/doc/diagrams/CodeGenClassDiagram.svg](./doc/diagrams/CodeGenClassDiagram.svg)
