# My Project - Frontend

## Overview

This frontend project is designed to offer a rich and interactive user experience by interfacing with our backend services. Built using modern frontend technologies, it provides a responsive and dynamic user interface that integrates seamlessly with the backend.

## Technologies Used

- **React**: A JavaScript library for building user interfaces.
- **Axios**: A promise-based HTTP client for making API requests.
- **React Router**: For handling routing and navigation within the application.
- **CSS Modules**: For scoped and modular styling of components.
- **JWT Authentication**: Secure user authentication using JSON Web Tokens (JWT).

## Key Features

### User Authentication

- **Login and Session Management**: Users log in with their credentials, which are sent to the backend. Upon successful authentication, a JWT token is stored in an HTTP-only cookie for secure session management. This token is used for subsequent requests to authenticate and authorize users.

### Role-Based Access Control

- **Dynamic Access**: The application adjusts available features based on the user's role. Admins have additional capabilities, such as editing client details and accessing sensitive information, while standard users have access to basic functionalities.

### Client Management

- **Client Details**: View detailed information about clients, including personal details, contact information, and associated contracts.
- **Edit and Delete**: Admins can modify client details and remove clients from the system, with changes confirmed through a user-friendly confirmation prompt.

### Software Revenue Page

- **Revenue Tracking**: Includes a dedicated page where users can view and analyze revenue for specific software. This feature helps in understanding financial performance and making informed decisions.

## Project Structure

Here’s an overview of the project directory:

- **`src/`**: Main source code directory.
  - **`components/`**: Contains reusable UI components.
  - **`pages/`**: Main pages of the application, including the login page and client details page.
  - **`hooks/`**: Custom hooks for managing state and side effects.
  - **`utils/`**: Utility functions and constants.
- **`public/`**: Publicly accessible assets such as images.
- **`package.json`**: Manages project dependencies and scripts.

## Getting Started

To get started with this frontend project:

### Prerequisites

Ensure Node.js is installed on your system. Download it from [nodejs.org](https://nodejs.org/).

### Installation

1. **Clone the repository**:

    ```bash
    git clone https://github.com/danilzagor/Revenue-Recognition-Application/
    cd Revenue-Recognition-Application/Frontend
    ```

2. **Install dependencies**:

    ```bash
    npm install
    ```

### Running the Application

Start the development server:

```bash
npm start
