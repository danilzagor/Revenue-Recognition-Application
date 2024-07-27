# Revenue Recognition System

This project is a REST API system designed to handle the complex issue of revenue recognition. The system ensures that revenues are accurately and compliantly recognized for various types of products, helping to maintain transparency and trust in financial reporting.

## Features

### Customer Management
- **Add Customer**: Store information about customers, who can be either individuals or companies.
  - For individuals: first name, last name, address, email, phone number, and PESEL (Polish Identification Number).
  - For companies: company name, address, email, phone number, and KRS number (National Court Register number).
- **Update Customer**: Modify customer information, except for PESEL for individuals and KRS for companies.
- **Soft Delete Customer**: Soft delete individual customers by overwriting their data while keeping the record in the database. Company data cannot be deleted.

### Software Licensing
- **Manage Software Products**: Store information about software products, including name, description, current version, and category (e.g., finance, education).
- **Apply Discounts**: Discounts can be applied to the purchase price of the software or the first period of a subscription. Discounts are time-bound and expressed in percentages.
  - Example: "Black Friday Discount" - 10% off from 01-01 to 03-03.

### Contract Management for Software Purchase
- **Create Contract**: Prepare a contract for purchasing software. The contract includes start and end dates (minimum 3 days, maximum 30 days), price, and any applicable discounts.
  - The contract also specifies the updates the customer will receive, with at least 1 year of updates included.
  - Additional support can be added for 1, 2, or 3 years at an extra cost of 1000 PLN per year.
- **Issue Payment**: Clients must fully pay within the contract's specified timeframe. Partial or late payments are not accepted.
- **Revenue Recognition**: Only after full payment can the contract value be treated as revenue.

### Revenue Calculation
- **Calculate Current Revenue**: Calculate revenue based on payments and signed contracts.
- **Calculate Expected Revenue**: Project future revenue assuming all pending contracts will be signed.
- **Currency Conversion**: Convert revenue to different currencies using a public exchange rate service.

## Technical Details
- **Entity Framework**: Used for database interactions.
- **Authentication and Authorization**: Endpoints are accessible only to logged-in employees. Admin users can edit and delete customers, while standard users have access to other functionalities.
- **API Documentation**: Swagger is used for API testing and documentation.
- **Design Patterns**: Best practices and design patterns are applied for maintainability and scalability.
- **Unit Testing**: Comprehensive unit tests for business logic.

## Project Structure
The project is structured to separate concerns and improve maintainability. It can be organized using either a vertical or horizontal slicing approach, with appropriate namespaces or separate projects.

## Usage
To use the API, users must log in with their credentials. Admin users have additional privileges for customer management. Standard users can manage software products, create contracts, and calculate revenue.

## Installation and Setup
1. Clone the repository from GitHub.
2. Restore the NuGet packages.
3. Set up the database connection string in the configuration file.
4. Run the database migrations.
5. Start the API server.

## Testing
Run unit or integration tests to ensure the business logic is functioning correctly. Use Swagger for manual API testing and documentation.


## License
This project is licensed under the MIT License.
