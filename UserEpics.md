# User Epics

## User Registration and Authentication (User Service)
**Operations:**
- Register new users with necessary information (username, email, password).
- Authenticate users based on provided credentials.
- Manage user profiles, including updating user details and preferences.
- Handle user roles and permissions for authorization.
- This service is being implmented using AWS Cognito, lambda and API Gateway.

## Project Management (Project Service)
**Operations:**
- Create new projects with name and description.
- Retrieve project details, including tasks and comments associated with the project.
- Update project information.
- Delete projects when needed.

## Task Management (Task Service)
**Operations:**
- Create new tasks with title, description, and assign them to team members.
- Retrieve task details, including comments for collaboration.
- Update task status, details, and assignment.
- Delete tasks when required.

## Comment Management (Comment Service)
**Operations:**
- Add comments on tasks to facilitate user collaboration.
- Retrieve comments with associated task and user details.
- Update or delete comments if necessary.

## Dashboard and Summary (Dashboard Service)
**Operations:**
- Provide aggregated data for the user dashboard, including task and project summaries.

## Search Functionality (Search Service)
**Operations:**
- Enable users to search for projects and tasks based on keywords and filters.

## Workflow Orchestration (Workflow Orchestration Service)
**Operations:**
- Allow administrators to define and configure custom workflows for various system components.
- Orchestrates the flow of workflows based on defined steps, transitions, conditions, and actions.
- Integrates with other services to execute custom workflows when specific events or conditions occur.

# Non-Functional Requirements
**Scalability:** The system should be able to scale up and down based on the load to handle varying user demands efficiently.

**Deployment Flexibility:** Any change in the system should be quickly and easily deployed on any platform without impacting other parts of the system.

**High Availability:** Business should not stop if any of the services or components are down. The system must be designed to ensure high availability and fault tolerance.

**Traceability:** Any issue related to a customer request should be quickly traced to its source for efficient debugging and resolution.

**Real-Time Metrics and Monitoring:** The system should provide real-time metrics of system performance, and alerts should be raised for any performance issues or failures.

# Communication Mechanisms
**End-User Communication:** The system will expose APIs for end-users to interact with various services. The APIs will be designed using RESTful principles, and communication will be over HTTP(S). Authentication for end-users will be handled using tokens, such as JSON Web Tokens (JWT), after successful registration and login.

**Service-to-Service Communication:** The services within the system will communicate with each other using asynchronous messaging patterns like Event Bus or Message Queue. When a service performs an action that may affect other services, it will publish events to the Event Bus or Message Queue. Other services subscribing to these events will then process the event accordingly. This decouples the services and ensures loose coupling, which is essential for microservices architecture.

# Project, Task, or Comment Submission and Notification
Assuming the "Project Service" handles project creation, when a user creates a new project, the "Project Service" will publish a "ProjectCreatedEvent" to the Event Bus or Message Queue. The "Task Service" and "Comment Service" can subscribe to these events to associate the project with tasks and comments. Similarly, when a user creates a new task or comment, the corresponding service will publish events related to those actions, and other relevant services will subscribe and take necessary actions.

# Implementation and Test Cases
For implementation, the chosen technologies are C#, .NET 6, xUnit for testing, CQRS with Mediatr, Fluent Validation, Repository Pattern, DDD, TDD, SOLID Principles, KISS, DRY, YAGNI, and Clean Architecture.

Each service will be developed as a separate microservice, following clean architecture principles. The code will be organized into separate projects for each service, and unit tests will be written using xUnit to ensure the functionality of individual components.

For project, task, or comment submission and notification, we'll implement event-based communication between services using an Event Bus or Message Queue.

The test cases will be written to cover different scenarios and edge cases, ensuring proper validation of inputs, handling of errors, and verifying the correct flow of events and data between services.

Overall, the system will be designed with modularity, scalability, and fault tolerance in mind to meet the specified Non-Functional Requirements and provide a robust and efficient microservices-based project management solution.
