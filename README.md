# CodeGenerator
Code generator with optional transformation rules

## Semestral project specification
The goal of the project is to create a web tool for generating code from class diagrams. The tool will allow generation into different languages e.g. JAVA, C#. It will be possible to add a new language by defining transformation rules. Also for one language the possibility of defining multiple options or on e.g. a specific constraint or part of the model. The expected output of the project is:

1. Functional source code in Java, C#, ...
2. Text part (10-20 pages)
3. Presentation for about 10 minutes
4. Data for experiments


### Contents of the text part
1. Introduction to the problem
2. State of the art
3. Detailed description of the selected part and its integration into the tool.
4. Experiments, evaluation (tables and graphs can be used)
5. Conclusion - evaluation of results

### Besides the main part, the work will also include familiarization and active use of e.g.
1. Java platform with Eclipse environment.
2. Management using the GIT versioning system.
3. Testing - according to the V model.
4. Code quality measurement - using Sonar Qube, ...
5. Task management using MANTIS.
6. Linking contributions to the versioning system ("commits") with tasks.
7. Reporting time worked on tasks.
8. Ensuring traceability.

### Docker-compose
You can simply run docker-compose up to run whole system at root directory of the project.

Client application url: localhost/Client (port 80)

Server url: localhost:5000

localhost:5000/swagger
