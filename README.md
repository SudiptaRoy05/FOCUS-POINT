FocusPoint
FocusPoint is a web-based photography marketplace developed as an undergraduate project and thesis. The platform is intended to bring photography buyers, photographers, and photography sellers together in one place.

It provides a digital space where users can browse and purchase photographs, offer photographs for sale, and find or hire photographers. The project explores how a web application can support the discovery and exchange of photography-related services and digital content.
Table of Contents
- Project Overview
- Project Objectives
- Core Platform Concept
- Technology Stack
- Application Architecture
- Development Tools
- Getting Started
- Configuration Notes
- Database
- Repository Structure
- Academic Project
- Future Improvements
Project Overview
Photography is both a creative service and a source of digital content. People may want to purchase photographs for personal or professional use, while photographers need a place to present their work and connect with potential clients.

FocusPoint was developed around this idea. It is a web application that brings these activities together in a single platform:
1. Buying photographs – users can use the platform to find photographs they are interested in purchasing.
2. Selling photographs – photographers or image creators can use the platform as a place to make their photographs available to potential buyers.
3. Hiring photographers – users who need photography services can use the platform to find and hire photographers.
The project's central idea is to make photography-related content and services easier to discover through a dedicated online marketplace.
Project Objectives
The main objectives of FocusPoint are:
- To develop a web-based marketplace focused on photography.
- To provide a platform for the buying and selling of photographs.
- To connect people looking for photography services with photographers.
- To organize the application using a structured, four-tier architecture.
- To use a relational database to store and manage application data.
- To apply software engineering concepts in the design and development of a complete web application.
Core Platform Concept
FocusPoint is designed around three main marketplace activities.
1. Photograph marketplace
The platform is intended to let users explore photographs that are available for purchase. This creates a dedicated space for photography buyers and image creators to connect.
2. Photograph selling
Photographers and other image creators can use the platform to make their photographs available to potential customers. The marketplace concept gives creators an online channel through which their work can be discovered.
3. Photographer hiring
Not every photography requirement can be met by purchasing an existing image. Users may instead need a photographer for a particular service or occasion. FocusPoint also addresses this need by providing a place to find and hire photographers.
Note: This README describes the project's confirmed purpose and technology. It intentionally does not claim specific features such as payment processing, chat, reviews, booking calendars, authentication methods, or administrator workflows unless those are verified in the source code.

Technology Stack
Area	Technology
Application framework	ASP.NET Core
Programming language	C#
Database	Microsoft SQL Server
Application architecture	Four-tier architecture
Code editor	Visual Studio Code
Version control	Git
Source code hosting	GitHub


Application Architecture
FocusPoint was organized using a four-tier architecture. The purpose of separating an application into tiers is to keep different responsibilities organized rather than placing all application logic in one part of the system.

The project uses this architectural approach to structure the web application and its interaction with the database.

When documenting or extending the implementation, use the actual project folders and namespaces to identify the exact responsibility of each tier. The names and boundaries of the four tiers are not specified here because they should match the implementation in the repository.
Development Tools
- Visual Studio Code was used as the development environment.
- Git was used for source-code version control.
- GitHub was used to host the project repository.
- Microsoft SQL Server was used as the relational database system.
Getting Started
The repository should be cloned to a local development machine before attempting to run the application.
Prerequisites
Install the tools required by the version of the project:
- .NET SDK compatible with the target framework specified in the project files.
- Microsoft SQL Server.
- Visual Studio Code or another compatible C# development environment.
- Git.
1. Clone the repository
git clone https://github.com/SudiptaRoy05/FOCUS-POINT.git
cd FOCUS-POINT
2. Open the project
Open the repository folder in Visual Studio Code:
code .
3. Configure the database connection
Configure the SQL Server connection string using the configuration approach expected by the application.

Do not commit database passwords, API keys, or other credentials to GitHub. Keep local secrets outside version control. If the application uses appsettings.json, provide a safe example configuration file with placeholder values rather than real credentials.
4. Restore dependencies
From the directory containing the solution or project file, run:
dotnet restore
5. Build the application
dotnet build
6. Run the application
Run the web project using its project path:
dotnet run --project path/to/your-web-project.csproj
Replace the example project path with the actual .csproj path in this repository.
Setup status: The exact .NET target version, solution/project filename, database name, connection-string key, and database initialization procedure have not been documented here. Check the .csproj, configuration files, and database scripts before using these commands as a final deployment guide.

Configuration Notes
Before running the application on a new machine:
1. Check the target framework in the .csproj file and install the matching .NET SDK.
2. Check the application's configuration files for required settings.
3. Configure a local SQL Server instance and the correct connection string.
4. Check whether the project uses Entity Framework migrations, SQL scripts, or another database initialization method.
5. Keep credentials and machine-specific configuration out of committed source files.
Database
FocusPoint uses Microsoft SQL Server as its database system.

The database supports the application's data-storage requirements. The exact table names, relationships, constraints, and database initialization steps should be documented from the database scripts or data-access code so that this section accurately reflects the implemented schema.
Repository Structure
The repository is organized into multiple project areas. The confirmed architecture is four-tier; the exact folder-to-tier mapping should be taken from the current source tree.

To inspect the repository structure locally, run:
find . -maxdepth 3 -type f
You can then update this section with the actual folder and project names, for example:
FOCUS-POINT/
├── <application-project>/
├── <other-project-or-layer>/
├── <solution-file>
└── README.md
The placeholders above are intentional and should be replaced with the real names from the repository.
Academic Project
FocusPoint was developed as an undergraduate project and thesis for the B.Sc. in Computer Science and Engineering program at Stamford University Bangladesh.

The project received an A grade.

It provided practical experience in developing a web application with ASP.NET Core and C#, working with Microsoft SQL Server, organizing application code through a four-tier architecture, and managing source code with Git and GitHub.
Future Improvements
Possible areas for future development of a photography marketplace include:
- More detailed search and filtering for photographs and photographers.
- Photographer portfolios and richer profile pages.
- Clearer service descriptions and booking workflows.
- Secure payment integration, if required by the product.
- User feedback and rating features.
- Improved accessibility, responsiveness, and usability.
- Automated testing and deployment workflows.
These are possible future enhancements, not a statement that they are already implemented in the current project.
Repository
GitHub: https://github.com/SudiptaRoy05/FOCUS-POINT
Developed by: Sudipta Roy Ballave