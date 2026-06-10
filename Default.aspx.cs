using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace Platespotter
{
    public partial class Default : System.Web.UI.Page
    {
        // Data models
        public class Cafe
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public string Image { get; set; }
            public double Rating { get; set; }
            public int Reviews { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public List<string> Specialties { get; set; }
            public string PriceRange { get; set; }
            public List<SocialProof> SocialProof { get; set; }
            public List<string> Delivery { get; set; }
        }

        public class SocialProof
        {
            public string Platform { get; set; }
            public string Count { get; set; }
            public string Text { get; set; }
        }

        public class Review
        {
            public int Id { get; set; }
            public int CafeId { get; set; }
            public string CafeName { get; set; }
            public string UserName { get; set; }
            public string UserImage { get; set; }
            public int Rating { get; set; }
            public string Date { get; set; }
            public string Comment { get; set; }
            public int Likes { get; set; }
            public string Platform { get; set; }
        }

        // Session keys
        private const string SESSION_CAFES = "CafesData";
        private const string SESSION_REVIEWS = "ReviewsData";
        private const string SESSION_USER_ID = "UserId";
        private const string SESSION_LOGGED_IN = "IsLoggedIn";

        // Connection string - UPDATED for Platespot database
        private string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Platespot"]?.ConnectionString ??
                       "Server=DESKTOP-LRGORL9\\SQLEXPRESS;Database=Platespot;Integrated Security=true;";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeData();

                // Only load restaurants if user is logged in
                if (IsUserLoggedIn())
                {
                    // Query string se tab check karein
                    string tab = Request.QueryString["tab"];
                    string success = Request.QueryString["success"];

                    if (!string.IsNullOrEmpty(success) && success == "true")
                    {
                        SuccessMessagePanel.Visible = true;
                    }

                    if (!string.IsNullOrEmpty(tab))
                    {
                        switch (tab)
                        {
                            case "favorites":
                                LoadFavoritesView();
                                break;
                            case "reviews":
                                LoadReviewsView();
                                break;
                            default:
                                LoadRestaurantsView();
                                break;
                        }
                    }
                    else
                    {
                        LoadRestaurantsView();
                    }
                }
            }
        }

        private void InitializeData()
        {
            if (Session[SESSION_CAFES] == null)
            {
                Session[SESSION_CAFES] = GetCafesFromDatabase();
            }

            if (Session[SESSION_REVIEWS] == null)
            {
                Session[SESSION_REVIEWS] = GetReviewsFromDatabase();
            }

            // Check login status
            if (Session[SESSION_LOGGED_IN] == null)
            {
                Session[SESSION_LOGGED_IN] = false;
            }

            // Set initial panel visibility based on login status
            if (IsUserLoggedIn())
            {
                ShowMainApp();
            }
            else
            {
                ShowLoginPage();
            }
        }

        private bool IsUserLoggedIn()
        {
            return Session[SESSION_LOGGED_IN] != null && (bool)Session[SESSION_LOGGED_IN];
        }

        private int GetCurrentUserId()
        {
            return Session[SESSION_USER_ID] != null ? (int)Session[SESSION_USER_ID] : -1;
        }

        // DATABASE METHODS - Get real data from database
        private List<Cafe> GetCafesFromDatabase()
        {
            var cafes = new List<Cafe>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT CafeID, CafeName, Category, Location, Description, 
                                   PriceRange, Specialties, Rating, ReviewCount, ImageUrl 
                                   FROM Cafes WHERE IsActive = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cafe = new Cafe
                                {
                                    Id = Convert.ToInt32(reader["CafeID"]),
                                    Name = reader["CafeName"].ToString(),
                                    Category = reader["Category"].ToString(),
                                    Location = reader["Location"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    PriceRange = reader["PriceRange"].ToString(),
                                    Rating = Convert.ToDouble(reader["Rating"]),
                                    Reviews = Convert.ToInt32(reader["ReviewCount"]),
                                    Image = reader["ImageUrl"].ToString() ?? "/images/default-cafe.jpg",
                                    Specialties = reader["Specialties"]?.ToString()?.Split(',').ToList() ?? new List<string>(),
                                    Delivery = new List<string> { "Foodpanda", "Cheetay" }, // Default delivery options
                                    SocialProof = new List<SocialProof>()
                                };

                                cafes.Add(cafe);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error and return empty list
                System.Diagnostics.Debug.WriteLine($"Error loading cafes: {ex.Message}");
            }

            return cafes;
        }

        private List<Review> GetReviewsFromDatabase()
        {
            var reviews = new List<Review>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // UPDATED QUERY: Using UserName from Reviews table instead of joining with Users
                    string query = @"SELECT r.ReviewID, r.CafeID, c.CafeName, r.Rating, r.Comment, 
                                   r.ReviewDate, r.UserName, r.Likes, r.Platform
                                   FROM Reviews r 
                                   INNER JOIN Cafes c ON r.CafeID = c.CafeID 
                                   WHERE r.IsActive = 1 
                                   ORDER BY r.ReviewDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var review = new Review
                                {
                                    Id = Convert.ToInt32(reader["ReviewID"]),
                                    CafeId = Convert.ToInt32(reader["CafeID"]),
                                    CafeName = reader["CafeName"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    Rating = Convert.ToInt32(reader["Rating"]),
                                    Comment = reader["Comment"].ToString(),
                                    Date = Convert.ToDateTime(reader["ReviewDate"]).ToString("MMM dd, yyyy"),
                                    Likes = Convert.ToInt32(reader["Likes"]),
                                    Platform = reader["Platform"].ToString(),
                                    UserImage = "/images/user-default.png"
                                };

                                reviews.Add(review);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading reviews: {ex.Message}");
            }

            return reviews;
        }

        // User Authentication Methods - UPDATED for any email
        private bool ValidateUser(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if user exists
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", email);
                        int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (userCount == 0)
                        {
                            // Create new user if doesn't exist
                            string insertQuery = @"INSERT INTO Users (Email, PasswordHash, FullName) 
                                                VALUES (@Email, @PasswordHash, @FullName)";

                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Email", email);
                                insertCommand.Parameters.AddWithValue("@PasswordHash", password); // Simple storage for demo
                                insertCommand.Parameters.AddWithValue("@FullName", "User");

                                insertCommand.ExecuteNonQuery();
                                return true;
                            }
                        }
                        else
                        {
                            // Check password for existing user
                            string passwordQuery = "SELECT PasswordHash FROM Users WHERE Email = @Email";
                            using (SqlCommand passwordCommand = new SqlCommand(passwordQuery, connection))
                            {
                                passwordCommand.Parameters.AddWithValue("@Email", email);
                                string storedPassword = passwordCommand.ExecuteScalar()?.ToString();

                                // Simple password check - in production use proper hashing
                                return password == storedPassword;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }

        private int GetUserId(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT UserId FROM Users WHERE Email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting user ID: {ex.Message}");
                return -1;
            }
        }

        // Favorites Management
        private bool AddToFavorites(int userId, int cafeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"IF NOT EXISTS (SELECT 1 FROM UserFavorites WHERE UserId = @UserId AND CafeId = @CafeId)
                                   INSERT INTO UserFavorites (UserId, CafeId) 
                                   VALUES (@UserId, @CafeId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@CafeId", cafeId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding favorite: {ex.Message}");
                return false;
            }
        }

        private bool RemoveFromFavorites(int userId, int cafeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM UserFavorites WHERE UserId = @UserId AND CafeId = @CafeId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@CafeId", cafeId);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing favorite: {ex.Message}");
                return false;
            }
        }

        private bool IsFavorite(int userId, int cafeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM UserFavorites WHERE UserId = @UserId AND CafeId = @CafeId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@CafeId", cafeId);

                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking favorite: {ex.Message}");
                return false;
            }
        }

        private List<int> GetUserFavorites(int userId)
        {
            var favorites = new List<int>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT CafeId FROM UserFavorites WHERE UserId = @UserId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                favorites.Add(Convert.ToInt32(reader["CafeId"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting favorites: {ex.Message}");
            }
            return favorites;
        }

        // Cafe Submission
        // NEW METHOD: Submit cafe to main Cafes table and auto-add to favorites
        private bool SubmitCafeToMain(int userId, string name, string category, string location,
                                    string priceRange, string description, string specialties)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // First, insert into main Cafes table
                    string insertCafeQuery = @"INSERT INTO Cafes 
                                    (CafeName, Category, Location, Description, PriceRange, Specialties, Rating, ReviewCount, ImageUrl, IsActive) 
                                    VALUES (@Name, @Category, @Location, @Description, @PriceRange, @Specialties, 0, 0, @ImageUrl, 1);
                                    SELECT SCOPE_IDENTITY();";

                    int newCafeId;
                    using (SqlCommand command = new SqlCommand(insertCafeQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@PriceRange", priceRange ?? "");
                        command.Parameters.AddWithValue("@Specialties", specialties ?? "");
                        command.Parameters.AddWithValue("@ImageUrl", "/images/default-cafe.jpg"); // Default image

                        newCafeId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Also add to UserSubmittedCafes for tracking
                    string trackQuery = @"INSERT INTO UserSubmittedCafes 
                                (UserId, Name, Category, Location, PriceRange, Description, Specialties, Status, SubmissionDate) 
                                VALUES (@UserId, @Name, @Category, @Location, @PriceRange, @Description, @Specialties, 'Approved', GETDATE())";

                    using (SqlCommand trackCommand = new SqlCommand(trackQuery, connection))
                    {
                        trackCommand.Parameters.AddWithValue("@UserId", userId);
                        trackCommand.Parameters.AddWithValue("@Name", name);
                        trackCommand.Parameters.AddWithValue("@Category", category);
                        trackCommand.Parameters.AddWithValue("@Location", location);
                        trackCommand.Parameters.AddWithValue("@PriceRange", priceRange ?? "");
                        trackCommand.Parameters.AddWithValue("@Description", description);
                        trackCommand.Parameters.AddWithValue("@Specialties", specialties ?? "");

                        trackCommand.ExecuteNonQuery();
                    }

                    // Auto-add to favorites
                    AddToFavorites(userId, newCafeId);

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error submitting cafe to main: {ex.Message}");
                return false;
            }
        }

        // Login/Logout functionality - UPDATED with redirect
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordTextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowErrorMessage("Please enter both email and password");
                return;
            }

            // Validate user - will create new user if doesn't exist
            if (ValidateUser(email, password))
            {
                int userId = GetUserId(email);

                Session[SESSION_LOGGED_IN] = true;
                Session[SESSION_USER_ID] = userId;

                // Refresh data from database
                Session[SESSION_CAFES] = GetCafesFromDatabase();
                Session[SESSION_REVIEWS] = GetReviewsFromDatabase();

                // REDIRECT AFTER LOGIN - Prevents resubmission warning
                SafeRedirect("Default.aspx?tab=restaurants");
            }
            else
            {
                ShowErrorMessage("Invalid login. Please try again.");
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Session[SESSION_LOGGED_IN] = false;
            Session[SESSION_USER_ID] = null;
            Session[SESSION_CAFES] = null;
            Session[SESSION_REVIEWS] = null;

            // REDIRECT AFTER LOGOUT
            SafeRedirect("Default.aspx");
        }

        // Helper method for safe redirect
        private void SafeRedirect(string url)
        {
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void ShowLoginPage()
        {
            LoginPanel.Visible = true;
            MainAppPanel.Visible = false;
        }

        private void ShowMainApp()
        {
            LoginPanel.Visible = false;
            MainAppPanel.Visible = true;
        }

        // Tab navigation
        protected void RestaurantsTab_Click(object sender, EventArgs e)
        {
            if (IsUserLoggedIn())
            {
                LoadRestaurantsView();
            }
        }

        protected void ReviewsTab_Click(object sender, EventArgs e)
        {
            if (IsUserLoggedIn())
            {
                LoadReviewsView();
            }
        }

        protected void FavoritesTab_Click(object sender, EventArgs e)
        {
            if (IsUserLoggedIn())
            {
                LoadFavoritesView();
            }
        }

        private void LoadRestaurantsView()
        {
            RestaurantsPanel.Visible = true;
            ReviewsPanel.Visible = false;
            FavoritesPanel.Visible = false;
            UpdateActiveTab(RestaurantsTab);

            var cafes = GetFilteredCafes();
            CafesRepeater.DataSource = cafes;
            CafesRepeater.DataBind();

            ResultsCountLiteral.Text = cafes.Count.ToString();
        }

        private void LoadReviewsView()
        {
            RestaurantsPanel.Visible = false;
            ReviewsPanel.Visible = true;
            FavoritesPanel.Visible = false;
            UpdateActiveTab(ReviewsTab);

            var reviews = GetFilteredReviews();
            ReviewsRepeater.DataSource = reviews;
            ReviewsRepeater.DataBind();
        }

        private void LoadFavoritesView()
        {
            RestaurantsPanel.Visible = false;
            ReviewsPanel.Visible = false;
            FavoritesPanel.Visible = true;
            UpdateActiveTab(FavoritesTab);

            var favorites = GetFavoriteCafes();
            FavoritesRepeater.DataSource = favorites;
            FavoritesRepeater.DataBind();

            EmptyFavoritesPanel.Visible = favorites.Count == 0;
        }

        private void UpdateActiveTab(LinkButton activeTab)
        {
            // Reset all tabs
            RestaurantsTab.CssClass = "py-2 px-4 border-b-2 border-transparent text-gray-500 hover:text-gray-700 font-medium text-sm";
            ReviewsTab.CssClass = "py-2 px-4 border-b-2 border-transparent text-gray-500 hover:text-gray-700 font-medium text-sm";
            FavoritesTab.CssClass = "py-2 px-4 border-b-2 border-transparent text-gray-500 hover:text-gray-700 font-medium text-sm";

            // Set active tab
            activeTab.CssClass = "py-2 px-4 border-b-2 border-orange-500 text-orange-600 font-medium text-sm";
        }

        // Favorites functionality - UPDATED with redirects
        protected void CafesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!IsUserLoggedIn()) return;

            int userId = GetCurrentUserId();
            int cafeId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ToggleFavorite")
            {
                if (IsFavorite(userId, cafeId))
                {
                    RemoveFromFavorites(userId, cafeId);
                }
                else
                {
                    AddToFavorites(userId, cafeId);
                }

                // REDIRECT AFTER DATABASE UPDATE - Prevents resubmission warning
                SafeRedirect("Default.aspx?tab=restaurants");
                return;
            }
            else if (e.CommandName == "ViewDetails")
            {
                ShowCafeModal(cafeId);
            }
        }

        protected void FavoritesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (!IsUserLoggedIn()) return;

            int userId = GetCurrentUserId();
            int cafeId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "RemoveFavorite")
            {
                RemoveFromFavorites(userId, cafeId);

                // REDIRECT AFTER DATABASE UPDATE - Prevents resubmission warning
                SafeRedirect("Default.aspx?tab=favorites");
                return;
            }
            else if (e.CommandName == "ViewDetails")
            {
                ShowCafeModal(cafeId);
            }
        }

        public bool IsFavorite(int cafeId)
        {
            if (!IsUserLoggedIn()) return false;

            int userId = GetCurrentUserId();
            return IsFavorite(userId, cafeId);
        }

        private List<Cafe> GetFavoriteCafes()
        {
            if (!IsUserLoggedIn()) return new List<Cafe>();

            int userId = GetCurrentUserId();
            var favoriteIds = GetUserFavorites(userId);
            var allCafes = (List<Cafe>)Session[SESSION_CAFES];

            return allCafes.Where(c => favoriteIds.Contains(c.Id)).ToList();
        }

        // Cafe submission - UPDATED with redirect
        protected void SubmitCafeButton_Click(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn())
            {
                ShowErrorMessage("Please login to submit a cafe");
                return;
            }

            if (ValidateNewCafeForm())
            {
                int userId = GetCurrentUserId();

                // Use the NEW method that adds to main Cafes table
                bool success = SubmitCafeToMain(
                    userId,
                    NewplaceName.Text.Trim(),
                    Category.SelectedValue,
                    Location.Text.Trim(),
                    PriceRange.Text.Trim(),
                    Description.Text.Trim(),
                    Specialties.Text.Trim()
                );

                if (success)
                {
                    // Refresh the cafes data to include the new cafe
                    Session[SESSION_CAFES] = GetCafesFromDatabase();

                    // REDIRECT AFTER DATABASE UPDATE - Prevents resubmission warning
                    SafeRedirect("Default.aspx?tab=favorites&success=true");
                    return;
                }
                else
                {
                    ShowErrorMessage("Error submitting cafe. Please try again.");
                }
            }
        }

        // Category filtering
        protected void CategoryButton_Click(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn()) return;

            var button = (LinkButton)sender;
            var category = button.CommandArgument;

            ViewState["CurrentCategory"] = category;
            LoadRestaurantsView();
            UpdateCategoryButtons(button);
        }

        private void UpdateCategoryButtons(LinkButton activeButton)
        {
            // Reset all category buttons
            AllCategoryButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg";
            HiddenGemsButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg";
            LocalFavoritesButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg";
            PopularChainsButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg";
            TopDeliveryButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg";

            // Set active category button
            activeButton.CssClass = "px-6 py-3 rounded-full font-medium transition-all duration-300 bg-orange-500 text-white shadow-lg transform scale-105";
        }

        // Review filtering
        protected void ReviewFilterButton_Click(object sender, EventArgs e)
        {
            if (!IsUserLoggedIn()) return;

            var button = (LinkButton)sender;
            var filter = button.CommandArgument;

            ViewState["ReviewFilter"] = filter;
            LoadReviewsView();
            UpdateReviewFilterButtons(button);
        }

        private void UpdateReviewFilterButtons(LinkButton activeButton)
        {
            // Reset all review filter buttons
            AllReviewsButton.CssClass = "px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50";
            PositiveReviewsButton.CssClass = "px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50";
            CriticalReviewsButton.CssClass = "px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50";

            // Set active review filter button
            activeButton.CssClass = "px-4 py-2 bg-orange-500 text-white rounded-lg";
        }

        // Search functionality
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (IsUserLoggedIn())
            {
                LoadRestaurantsView();
            }
        }

        // Data filtering methods
        private List<Cafe> GetFilteredCafes()
        {
            var allCafes = (List<Cafe>)Session[SESSION_CAFES];
            var filteredCafes = allCafes;

            // Apply category filter
            string currentCategory = ViewState["CurrentCategory"] as string;
            if (!string.IsNullOrEmpty(currentCategory) && currentCategory != "all")
            {
                filteredCafes = filteredCafes.Where(c => c.Category == currentCategory).ToList();
            }

            // Apply search filter
            string searchTerm = SearchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredCafes = filteredCafes.Where(c =>
                    c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Location.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Specialties.Any(s => s.ToLower().Contains(searchTerm.ToLower()))
                ).ToList();
            }

            return filteredCafes;
        }

        private List<Review> GetFilteredReviews()
        {
            var allReviews = (List<Review>)Session[SESSION_REVIEWS];
            var filteredReviews = allReviews;

            string reviewFilter = ViewState["ReviewFilter"] as string;
            if (!string.IsNullOrEmpty(reviewFilter))
            {
                if (reviewFilter == "positive")
                {
                    filteredReviews = filteredReviews.Where(r => r.Rating >= 4).ToList();
                }
                else if (reviewFilter == "negative")
                {
                    filteredReviews = filteredReviews.Where(r => r.Rating <= 3).ToList();
                }
            }

            return filteredReviews;
        }

        // UI helper methods
        public string RenderStarRating(double rating)
        {
            StringBuilder stars = new StringBuilder();
            stars.Append("<div class=\"flex items-center\">");

            for (int i = 0; i < 5; i++)
            {
                string starClass = i < Math.Floor(rating) ? "text-yellow-400" : "text-gray-300";
                stars.Append($@"<svg class=""w-4 h-4 {starClass}"" fill=""currentColor"" viewBox=""0 0 20 20"">
                    <path d=""M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"" />
                </svg>");
            }

            stars.Append($"<span class=\"ml-1 text-sm font-medium text-gray-700\">{rating}</span>");
            stars.Append("</div>");
            return stars.ToString();
        }

        public string GetCategoryClass(string category)
        {
            return category switch
            {
                "hidden-gem" => "px-3 py-1 rounded-full text-xs font-medium bg-purple-500 text-white",
                "local-favorite" => "px-3 py-1 rounded-full text-xs font-medium bg-green-500 text-white",
                "branded" => "px-3 py-1 rounded-full text-xs font-medium bg-blue-500 text-white",
                "foodpanda" => "px-3 py-1 rounded-full text-xs font-medium bg-orange-500 text-white",
                _ => "px-3 py-1 rounded-full text-xs font-medium bg-gray-500 text-white"
            };
        }

        public string GetCategoryText(string category)
        {
            return category switch
            {
                "hidden-gem" => "Hidden Gem",
                "local-favorite" => "Local Favorite",
                "branded" => "Popular Chain",
                "foodpanda" => "Top Delivery",
                _ => "All Restaurants"
            };
        }

        public string GetDeliveryClass(string service)
        {
            return service switch
            {
                "Foodpanda" => "px-2 py-1 text-xs rounded-full bg-green-100 text-green-800",
                "Cheetay" => "px-2 py-1 text-xs rounded-full bg-blue-100 text-blue-800",
                "Bykea" => "px-2 py-1 text-xs rounded-full bg-purple-100 text-purple-800",
                _ => "px-2 py-1 text-xs rounded-full bg-gray-100 text-gray-800"
            };
        }

        private void ShowCafeModal(int cafeId)
        {
            var cafe = ((List<Cafe>)Session[SESSION_CAFES]).FirstOrDefault(c => c.Id == cafeId);
            if (cafe != null)
            {
                string modalContent = GenerateModalContent(cafe);
                ModalContentLiteral.Text = modalContent;

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal",
                    "document.getElementById('CafeModalPanel').classList.remove('hidden');", true);
            }
        }

        private string GenerateModalContent(Cafe cafe)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($@"
                <div class='relative'>
                    <img src='{cafe.Image}' alt='{cafe.Name}' class='w-full h-64 object-cover rounded-t-2xl'>
                    <button onclick=""document.getElementById('CafeModalPanel').classList.add('hidden')"" class='absolute top-4 right-4 bg-white p-2 rounded-full shadow-lg'>
                        <svg class='w-6 h-6' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
                            <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M6 18L18 6M6 6l12 12' />
                        </svg>
                    </button>
                </div>
                
                <div class='p-6'>
                    <div class='flex justify-between items-start mb-4'>
                        <div>
                            <h2 class='text-2xl font-bold text-gray-900 mb-2'>{cafe.Name}</h2>
                            <div class='flex items-center text-gray-600'>
                                <svg class='w-4 h-4 mr-1' fill='none' stroke='currentColor' viewBox='0 0 24 24'>
                                    <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z' />
                                    <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M15 11a3 3 0 11-6 0 3 3 0 016 0z' />
                                </svg>
                                {cafe.Location}
                            </div>
                            <span class='{GetCategoryClass(cafe.Category)}'>{GetCategoryText(cafe.Category)}</span>
                        </div>
                        {RenderStarRating(cafe.Rating)}
                    </div>
                    
                    <p class='text-gray-700 mb-6 text-lg'>{cafe.Description}</p>
                    
                    <div class='grid grid-cols-1 md:grid-cols-2 gap-6 mb-6'>
                        <div>
                            <h3 class='font-semibold text-gray-800 mb-3'>Specialties</h3>
                            <div class='space-y-2'>");

            foreach (var specialty in cafe.Specialties)
            {
                sb.Append($@"
                                <div class='flex items-center'>
                                    <div class='w-2 h-2 bg-orange-500 rounded-full mr-3'></div>
                                    <span class='text-gray-700'>{specialty}</span>
                                </div>");
            }

            sb.Append($@"
                            </div>
                        </div>
                        
                        <div>
                            <h3 class='font-semibold text-gray-800 mb-3'>Price Range</h3>
                            <div class='flex items-center'>
                                <span class='text-2xl font-bold text-gray-900 mr-2'>{cafe.PriceRange}</span>
                            </div>
                            
                            <h3 class='font-semibold text-gray-800 mb-3 mt-4'>Delivery Options</h3>
                            <div class='flex flex-wrap gap-2'>");

            if (cafe.Delivery.Any())
            {
                foreach (var service in cafe.Delivery)
                {
                    sb.Append($@"<span class='{GetDeliveryClass(service)}'>{service}</span>");
                }
            }
            else
            {
                sb.Append(@"<span class='px-3 py-1 bg-gray-100 text-gray-800 text-sm rounded-full'>Dine-in Only</span>");
            }

            sb.Append($@"
                            </div>
                        </div>
                    </div>
                    
                    <div class='flex flex-wrap gap-3'>
                        <button onclick='toggleFavorite({cafe.Id})' class='flex-1 px-4 py-3 rounded-lg font-medium transition-all duration-300 {(IsFavorite(cafe.Id) ? "bg-red-500 text-white" : "bg-gray-200 text-gray-800 hover:bg-red-100")}'>
                            {(IsFavorite(cafe.Id) ? "❤️ Favorited" : "🤍 Add to Favorites")}
                        </button>
                        <button class='flex-1 bg-gradient-to-r from-orange-500 to-red-500 text-white px-4 py-3 rounded-lg font-medium hover:from-orange-600 hover:to-red-600 transition-all duration-300'>
                            Get Directions
                        </button>
                    </div>
                </div>");

            return sb.ToString();
        }

        // Clear form button click
        protected void ClearFormButton_Click(object sender, EventArgs e)
        {
            NewplaceName.Text = "";
            Location.Text = "";
            PriceRange.Text = "";
            Description.Text = "";
            Specialties.Text = "";
            Category.SelectedIndex = 0;
        }

        private bool ValidateNewCafeForm()
        {
            if (string.IsNullOrWhiteSpace(NewplaceName.Text))
            {
                ShowErrorMessage("Please enter a place name");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Location.Text))
            {
                ShowErrorMessage("Please enter the place location");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description.Text))
            {
                ShowErrorMessage("Please enter a description");
                return false;
            }

            return true;
        }

        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowError",
                $"alert('{message}');", true);
        }

        // Optional: Check if cafe already exists to avoid duplicates
        private bool CafeExists(string cafeName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Cafes WHERE CafeName = @CafeName AND IsActive = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CafeName", cafeName);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking cafe existence: {ex.Message}");
                return false;
            }
        }
    }
}