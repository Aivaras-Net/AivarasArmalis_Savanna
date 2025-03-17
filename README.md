# AivarasArmalis_Savanna

## Savanna Game Simulation

An ecosystem simulation game featuring various animals in a savanna environment.

## Admin Settings Configuration

The application requires admin user credentials to be configured through a `.env` file:

### Setting Up Admin Credentials (.env file)

1. Copy the `.env.template` file from the `src/Savanna.Web` directory to a new file named `.env` in the same directory
2. Set the following values in the `.env` file:
   ```
   ADMIN_EMAIL=your_email@example.com
   ADMIN_USERNAME=your_admin_username
   ADMIN_PASSWORD=YourSecurePassword123!
   ```

⚠️ **Important**: The application will not start without a properly configured `.env` file containing all required admin settings.

### Security Notice

The `.env` file contains sensitive information and should never be committed to source control. It is already included in the `.gitignore` file to prevent accidental commits.
