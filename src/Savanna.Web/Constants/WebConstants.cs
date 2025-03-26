namespace Savanna.Web.Constants
{
    public static class WebConstants
    {
        public const int TimerInterval = 500;
        public const int MaxLogEntries = 100;
        public const string GameStartedMessage = "Game started";
        public const string GameStoppedMessage = "Game stopped";
        public const string GamePausedMessage = "Game paused";
        public const string GameResumedMessage = "Game resumed";

        public const string AntelopeSpawnedMessage = "Antelope spawned at ({0}, {1})";
        public const string LionSpawnedMessage = "Lion spawned at ({0}, {1})";
        public const string FailedToSpawnAntelopeMessage = "Failed to spawn Antelope";
        public const string FailedToSpawnLionMessage = "Failed to spawn Lion";

        public const string SwitchedToLetterDisplayMessage = "Switched to letter display";
        public const string SwitchedToIconDisplayMessage = "Switched to icon display";

        public const string LogTimeFormat = "HH:mm:ss";

        public const string NavigatingToMessage = "Navigating to: {0}";
        public const string NavigationErrorMessage = "Navigation error: {0}";
        public const string JavaScriptInitializationErrorMessage = "JavaScript initialization error: {0}";

        public const string InvalidLoginAttemptMessage = "Invalid login attempt";
        public const string InvalidPasswordMessage = "Invalid password";
        public const string ErrorOccurredMessage = "An error occurred: {0}";
        public const string AllFieldsRequiredMessage = "All fields are required";
        public const string PasswordMismatchMessage = "The password and confirmation password do not match";

        public const string UserRoleName = "User";
        public const string AdminRoleName = "Admin";

        public const string GamePageInitializedMessage = "Game page initialized";
        public const string ModalInitializedSuccessfullyMessage = "Modal initialized successfully";
        public const string ErrorInitializingModalMessage = "Error initializing modal:";
        public const string BootstrapNotAvailableMessage = "Bootstrap not available, modal functionality may be limited";
        public const string NavigationWarningEnabledMessage = "Navigation warning enabled";
        public const string NavigationWarningDisabledMessage = "Navigation warning disabled";
        public const string NavigationInterceptedMessage = "Navigation intercepted to:";
        public const string ErrorInitializingDropdownsMessage = "Error initializing dropdowns:";
        public const string ErrorTogglingDropdownMessage = "Error toggling dropdown:";
        public const string ErrorShowingModalMessage = "Error showing modal:";

        public const string GameInProgressWarningTitle = "Warning: Game in Progress";
        public const string LeaveGameWarningMessage = "You will lose your game progress if you leave this page. Are you sure you want to leave?";
        public const string GameStateNotSavedMessage = "Your animals and game state will not be saved.";
        public const string StayOnPageButtonText = "Stay on this Page";
        public const string LeaveGameButtonText = "Leave Game";

        public const string NotProvidedText = "Not provided";
        public const string VerifiedBadgeText = "Verified";
        public const string NotVerifiedBadgeText = "Not Verified";
        public const string ActiveStatusText = "Active";
        public const string LockedStatusText = "Locked";
        public const string EnabledText = "Enabled";
        public const string DisabledText = "Disabled";
        public const string NoRolesAssignedText = "No roles assigned";

        public const string HelloUserText = "Hello, {0}!";

        public const string CreatedRoleLogMessage = "Created role: {0}";
        public const string FailedCreateRoleLogMessage = "Failed to create role {0}: {1}";
        public const string CreatedAdminUserLogMessage = "Created admin user with email: {0}";
        public const string AssignedAdminRoleLogMessage = "Assigned Admin role to admin user";
        public const string AdminUserVerificationLogMessage = "Admin user verification - Found: true, Has Admin role: {0}";
        public const string AdminUserVerificationFailedLogMessage = "Admin user verification failed - user not found";
        public const string FailedAssignRoleLogMessage = "Failed to assign Admin role: {0}";
        public const string FailedCreateUserLogMessage = "Failed to create admin user: {0}";
        public const string AssignedAdminRoleExistingUserLogMessage = "Assigned Admin role to existing admin user";
        public const string ExistingAdminUserLogMessage = "Existing admin user found - Email: {0}, Has Admin role: {1}";
        public const string CreatingAdminUserMessage = "Creating admin user with email: {0} from configuration";

        public const string StartingAppInitLogMessage = "Starting application initialization";
        public const string AppInitCompletedLogMessage = "Application initialization completed successfully";
        public const string AppInitErrorLogMessage = "An error occurred during application initialization";

        public const string EnsuringDbCreatedLogMessage = "Ensuring database is created";
        public const string DbCreationCompleteLogMessage = "Database creation check completed successfully";
        public const string DbCreationErrorLogMessage = "An error occurred while ensuring database is created";

        public const string ResumeButtonText = "Resume";
        public const string PauseButtonText = "Pause";
        public const string StopGameButtonText = "Stop Game";
        public const string SpawnAntelopeButtonText = "Spawn Antelope";
        public const string SpawnLionButtonText = "Spawn Lion";
        public const string ShowIconsButtonText = "Show Icons";
        public const string ShowLettersButtonText = "Show Letters";
        public const string StartNewGameButtonText = "Start New Game";

        public const string GameStatusText = "Game Status:";
        public const string PausedStatusText = "Paused";
        public const string RunningStatusText = "Running";
        public const string AnimalsCountText = "Animals:";
        public const string LionsCountText = "Lions:";
        public const string AntelopesCountText = "Antelopes:";
        public const string GameLogText = "Game Log";

        public const string AdminSettingsNotFoundMessage = "Admin settings not found in .env file. Please create a .env file with ADMIN_EMAIL, ADMIN_USERNAME, and ADMIN_PASSWORD.";
        public const string ConnectionStringNotFoundMessage = "Connection string 'DefaultConnection' not found.";
        public const string ErrorLoadingAnimalConfigMessage = "Error loading animal configuration";
        public const string AdminSettingsLoadedMessage = "Admin settings loaded from .env file:";
        public const string AdminEmailLogFormat = "  Email: {0}";
        public const string AdminUsernameLogFormat = "  Username: {0}";
        public const string AdminPasswordMaskedLogFormat = "  Password: {0}*** (masked)";
        public const string ApplicationInitErrorMessage = "An error occurred during application initialization.";

        public const string AnimalDetailsTitle = "Animal Details";
        public const string SpeciesLabel = "Species";
        public const string PositionLabel = "Position";
        public const string HealthLabel = "Health";
        public const string AgeLabel = "Age";
        public const string OffspringLabel = "Offspring";
        public const string CloseButtonText = "Close";
        public const string AgeUnitsText = "time units";

        public const int CellSize = 20;
        public const int AnimalDetailsUpdateInterval = 100;
        public const int LogContainerHeight = 150;

        public const string GameFieldBackgroundColor = "#e8d2a0";
        public const string LogContainerBackgroundColor = "#f8f9fa";
        public const string LionColor = "#cc3300";
        public const string AntelopeColor = "#33cc33";
        public const string SelectedAnimalGlowColor = "#00FFFF";
    }
}