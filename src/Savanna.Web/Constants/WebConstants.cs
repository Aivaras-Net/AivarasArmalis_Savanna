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

        public const string NoActiveGameToSaveMessage = "No active game to save";
        public const string GameStateSerialized = "Game state serialized";
        public const string InvalidGameStateFormat = "Invalid game state format";
        public const string GameStateLoadedMessage = "Game state loaded";
        public const string FailedToLoadAnimalTypeMessage = "Failed to load animal of type: {0}";
        public const string FailedToDeserializeGameStateMessage = "Failed to deserialize game state: {0}";
        public const string SaveGameSuccessMessage = "Game saved successfully";
        public const string SaveGameErrorMessage = "Error saving game: {0}";
        public const string LoadGameSuccessMessage = "Game loaded successfully";
        public const string LoadGameErrorMessage = "Error loading game: {0}";

        public const string StartingDatabaseInitMessage = "Starting database initialization...";
        public const string DatabaseInitCompletedMessage = "Database initialization completed successfully";
        public const string StartingRolesInitMessage = "Starting roles initialization...";
        public const string RolesInitCompletedMessage = "Roles initialization completed successfully";
        public const string StartingUsersInitMessage = "Starting default users initialization...";
        public const string UsersInitCompletedMessage = "Default users initialization completed successfully";

        public const string CreatedGameSaveLogMessage = "Created game save {0} for user {1}";
        public const string FailedDeleteSaveLogMessage = "Failed to delete save {0}. Save not found or doesn't belong to user {1}";
        public const string DeletedGameSaveLogMessage = "Deleted game save {0} for user {1}";

        public const string SelectedAnimalLogMessage = "Selected animal: {0} at ({1}, {2})";
        public const string ErrorSelectingAnimalMessage = "Error selecting animal: {0}";
        public const string FailedDeserializeGameStateMessage = "Failed to deserialize game state";

        public const string ErrorGettingSavesMessage = "Error getting saves";
        public const string ErrorSavingGameMessage = "Error saving game";
        public const string ErrorLoadingGameMessage = "Error loading game";
        public const string ErrorDeletingSaveMessage = "Error deleting save";

        public const string LoginPath = "/Account/AccessDenied";
        public const string LogoutPath = "/Account/Logout";
        public const string AccessDeniedPath = "/Account/AccessDenied";
        public const string ErrorPath = "/Error";
        public const string DefaultReturnPath = "/";

        public const string AdminEmailEnvVar = "ADMIN_EMAIL";
        public const string AdminUsernameEnvVar = "ADMIN_USERNAME";
        public const string AdminPasswordEnvVar = "ADMIN_PASSWORD";

        public const string DefaultConnectionString = "DefaultConnection";
        public const string IdentitySignInConfigPath = "Identity:SignIn";
        public const string IdentityPasswordConfigPath = "Identity:Password";

        public const string SaveDateFormat = "yyyy-MM-dd HH:mm:ss";

        public const string PluginsDirectory = "Plugins";
        public const string PluginFileSearchPattern = "*.dll";
        public const string PluginLoadedMessage = "Loaded plugin animal: {0}";
        public const string PluginLoadFailedMessage = "Failed to load plugin: {0}. Error: {1}";
        public const string PluginAnimalSpawnedMessage = "{0} spawned at ({1}, {2})";
        public const string FailedToSpawnPluginAnimalMessage = "Failed to spawn {0}";
        public const string PluginsPath = "/admin/plugins";
        public const string PluginDetailsPath = "/admin/plugins/{0}";

        public const string ErrorShowingNavigationWarningModalMessage = "Error showing navigation warning modal: {0}";
        public const string ErrorSettingBeforeUnloadWarningMessage = "Error setting beforeunload warning: {0}";
        public const string SaveNotFoundMessage = "Save not found";

        public const string GameSaveDeletedMessage = "Game save '{0}' deleted";
        public const string FailedToLoadSavedGamesMessage = "Failed to load saved games: {0}";
        public const string PleaseEnterSaveNameMessage = "Please enter a name for your save";

        public const string AnimalManuallyRemovedMessage = "{0} was manually removed from the simulation.";
        public const string NewOffspringCreatedMessage = "New {0} offspring was manually created.";
        public const string ErrorLoadingPluginAnimalsMessage = "Error loading plugin animals: {0}";
        public const string ErrorLoadingAnimalDetailsMessage = "Error loading animal details: {0}";

        public const string ErrorTitleText = "Error";
        public const string SuccessTitleText = "Success";
        public const string WarningTitleText = "Warning";
        public const string InfoTitleText = "Information";

        public const string RoleAddedSuccessMessage = "Role '{0}' added successfully.";
        public const string FailedToAddRoleMessage = "Failed to add role: {0}";
        public const string RoleRemovedSuccessMessage = "Role '{0}' removed successfully.";
        public const string FailedToRemoveRoleMessage = "Failed to remove role: {0}";
        public const string FailedToLockAccountMessage = "Failed to lock account: {0}";
        public const string FailedToUnlockAccountMessage = "Failed to unlock account: {0}";

        public const string ErrorUpdatingProfileMessage = "Error updating profile: {0}";
        public const string ErrorChangingPasswordMessage = "Error changing password: {0}";
        public const string ErrorUpdatingEmailMessage = "Error updating email: {0}";
        public const string ProfileUpdatedSuccessMessage = "Profile updated successfully.";
        public const string PasswordChangedSuccessMessage = "Password changed successfully.";
        public const string EmailUpdatedSuccessMessage = "Email updated successfully.";
        public const string AllPasswordFieldsRequiredMessage = "Please fill in all password fields.";
        public const string PasswordMismatchNewConfirmMessage = "The new password and confirmation password do not match.";
        public const string EmailCannotBeEmptyMessage = "Email cannot be empty.";
        public const string EmailSameAsCurrentMessage = "The new email is the same as the current email.";
        public const string AccountLockedSuccessMessage = "Account locked successfully.";
        public const string AccountUnlockedSuccessMessage = "Account unlocked successfully.";
        public const string CannotLockLastAdminMessage = "Cannot lock the last Admin user account.";
        public const string CannotRemoveLastAdminMessage = "Cannot remove the last Admin user from the system.";
    }
}