<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Platespotter.Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Hyderabad Food Guide - Discover the Best Food Spots in Hyderabad, Pakistan</title>
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="site.css" />
</head>
<body class="min-h-screen bg-gradient-to-br from-orange-50 to-red-50">
    <form id="form1" runat="server">
        <!-- Login Page -->
        <asp:Panel ID="LoginPanel" runat="server" CssClass="login-container">
            <div class="login-card">
                <div class="login-header">
                    <div class="login-icon">
                        <svg class="login-icon-svg" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                        </svg>
                    </div>
                    <h1 class="login-title">Hyderabad Food Guide</h1>
                    <p class="login-subtitle">Discover the Flavors of Hyderabad</p>
                </div>
                
                <div class="login-form">
                    <div class="login-form-group">
                        <label for="email" class="login-form-label">Email</label>
                        <asp:TextBox ID="EmailTextBox" runat="server" CssClass="login-form-input" placeholder="Enter your email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                            ControlToValidate="EmailTextBox"
                            ErrorMessage="Email is required"
                            CssClass="text-red-500 text-xs mt-1"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                    
                    <div class="login-form-group">
                        <label for="password" class="login-form-label">Password</label>
                        <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="login-form-input" placeholder="Enter your password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                            ControlToValidate="PasswordTextBox"
                            ErrorMessage="Password is required"
                            CssClass="text-red-500 text-xs mt-1"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                    
                    <div class="login-options">
                        <div class="remember-me">
                            <asp:CheckBox ID="RememberCheckBox" runat="server" CssClass="login-checkbox" />
                            <label for="<%= RememberCheckBox.ClientID %>" class="checkbox-label">Remember me</label>
                        </div>
                        <a href="#" class="forgot-password">Forgot password?</a>
                    </div>
                    
                    <asp:Button ID="LoginButton" runat="server" Text="Sign In" 
                        CssClass="login-button"
                        OnClick="LoginButton_Click" />
                    
                    <div class="login-footer">
                        <p class="signup-text">Don't have an account? <a href="#" class="signup-link">Sign up</a></p>
                    </div>
                </div>
            </div>
            
            <!-- Background Animation -->
            <div class="login-background">
                <div class="floating-food-icon">🍛</div>
                <div class="floating-food-icon">🍗</div>
                <div class="floating-food-icon">🍜</div>
                <div class="floating-food-icon">🍴</div>
                <div class="floating-food-icon">🌮</div>
            </div>
        </asp:Panel>

        <!-- Main App -->
        <asp:Panel ID="MainAppPanel" runat="server" Visible="false">
            <!-- Header -->
            <header class="bg-white shadow-lg sticky top-0 z-50">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
                    <div class="flex flex-col md:flex-row md:items-center md:justify-between">
                        <div class="flex items-center mb-4 md:mb-0">
                            <div class="bg-gradient-to-r from-orange-500 to-red-500 p-3 rounded-full mr-4">
                                <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                </svg>
                            </div>
                            <div>
                                <h1 class="text-3xl font-bold text-gray-900">Hyderabad Food Guide</h1>
                                <p class="text-gray-600">Discover the Flavors of Hyderabad</p>
                            </div>
                        </div>
                        
                        <!-- Navigation Tabs -->
                        <div class="flex space-x-4 mb-4 md:mb-0">
                            <asp:LinkButton ID="RestaurantsTab" runat="server" 
                                CssClass="py-2 px-4 border-b-2 border-orange-500 text-orange-600 font-medium text-sm"
                                OnClick="RestaurantsTab_Click">Restaurants</asp:LinkButton>
                            <asp:LinkButton ID="ReviewsTab" runat="server" 
                                CssClass="py-2 px-4 border-b-2 border-transparent text-gray-500 hover:text-gray-700 font-medium text-sm"
                                OnClick="ReviewsTab_Click">Reviews</asp:LinkButton>
                            <asp:LinkButton ID="FavoritesTab" runat="server" 
                                CssClass="py-2 px-4 border-b-2 border-transparent text-gray-500 hover:text-gray-700 font-medium text-sm"
                                OnClick="FavoritesTab_Click">Favorites</asp:LinkButton>
                        </div>
                        
                        <!-- Search and Buttons -->
                        <div class="flex items-center space-x-4">
                            <div class="relative">
                                <asp:TextBox ID="SearchTextBox" runat="server" 
                                    placeholder="Search restaurants, dishes, locations..."
                                    CssClass="w-full md:w-80 pl-10 pr-4 py-3 border border-gray-300 rounded-full focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-transparent" />
                                <svg class="absolute left-3 top-3.5 h-5 w-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                                </svg>
                            </div>
                            <asp:Button ID="SearchButton" runat="server" Text="Search" 
                                CssClass="bg-orange-500 hover:bg-orange-600 text-white px-4 py-2 rounded-full transition-colors"
                                OnClick="SearchButton_Click" />
                            <asp:Button ID="LogoutButton" runat="server" Text="Logout" 
                                CssClass="bg-red-500 hover:bg-red-600 text-white p-3 rounded-full transition-colors"
                                OnClick="LogoutButton_Click" />
                        </div>
                    </div>
                </div>
            </header>

            <!-- Main Content Area -->
            <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <!-- Restaurants Content -->
                <asp:Panel ID="RestaurantsPanel" runat="server">
                    <!-- Categories -->
                    <div class="flex flex-wrap gap-3 mb-8">
                        <asp:LinkButton ID="AllCategoryButton" runat="server" 
                            CssClass="px-6 py-3 rounded-full font-medium transition-all duration-300 bg-orange-500 text-white shadow-lg transform scale-105"
                            CommandArgument="all" OnClick="CategoryButton_Click">
                            <span class="mr-2">🍽️</span>All Restaurants
                        </asp:LinkButton>
                        <asp:LinkButton ID="HiddenGemsButton" runat="server" 
                            CssClass="px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg"
                            CommandArgument="hidden-gem" OnClick="CategoryButton_Click">
                            <span class="mr-2">💎</span>Hidden Gems
                        </asp:LinkButton>
                        <asp:LinkButton ID="LocalFavoritesButton" runat="server" 
                            CssClass="px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg"
                            CommandArgument="local-favorite" OnClick="CategoryButton_Click">
                            <span class="mr-2">❤️</span>Local Favorites
                        </asp:LinkButton>
                        <asp:LinkButton ID="PopularChainsButton" runat="server" 
                            CssClass="px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg"
                            CommandArgument="branded" OnClick="CategoryButton_Click">
                            <span class="mr-2">🏢</span>Popular Chains
                        </asp:LinkButton>
                        <asp:LinkButton ID="TopDeliveryButton" runat="server" 
                            CssClass="px-6 py-3 rounded-full font-medium transition-all duration-300 bg-white text-gray-700 hover:bg-orange-100 shadow-md hover:shadow-lg"
                            CommandArgument="foodpanda" OnClick="CategoryButton_Click">
                            <span class="mr-2">📦</span>Top Delivery
                        </asp:LinkButton>
                    </div>

                    <!-- Results count -->
                    <div class="mb-6">
                        <h2 class="text-2xl font-bold text-gray-900">
                            <asp:Literal ID="ResultsCountLiteral" runat="server" /> authentic restaurants found
                        </h2>
                        <p class="text-gray-600">Comprehensive guide to Hyderabad's best dining experiences</p>
                    </div>

                    <!-- Cafe Grid -->
                    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                        <asp:Repeater ID="CafesRepeater" runat="server" OnItemCommand="CafesRepeater_ItemCommand">
                            <ItemTemplate>
                                <div class="bg-white rounded-2xl shadow-xl overflow-hidden hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2">
                                    <div class="relative">
                                        <img src='<%# Eval("Image") %>' alt='<%# Eval("Name") %>' class="w-full h-48 object-cover">
                                        <asp:LinkButton ID="FavoriteButton" runat="server" 
                                            CommandName="ToggleFavorite" 
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass='<%# IsFavorite((int)Eval("Id")) ? "absolute top-4 right-4 p-2 rounded-full transition-all duration-300 bg-red-500 text-white" : "absolute top-4 right-4 p-2 rounded-full transition-all duration-300 bg-white text-gray-600 hover:bg-red-100" %>'>
                                            <svg class="w-5 h-5" fill='<%# IsFavorite((int)Eval("Id")) ? "currentColor" : "none" %>' stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                                            </svg>
                                        </asp:LinkButton>
                                        <div class="absolute bottom-4 left-4">
                                            <span class='<%# GetCategoryClass(Eval("Category").ToString()) %>'><%# GetCategoryText(Eval("Category").ToString()) %></span>
                                        </div>
                                    </div>
                                    
                                    <div class="p-6">
                                        <div class="flex justify-between items-start mb-3">
                                            <h3 class="text-xl font-bold text-gray-900"><%# Eval("Name") %></h3>
                                            <%# RenderStarRating((double)Eval("Rating")) %>
                                        </div>
                                        
                                        <p class="text-gray-600 mb-4 line-clamp-2"><%# Eval("Description") %></p>
                                        
                                        <div class="flex items-center text-sm text-gray-500 mb-4">
                                            <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                            </svg>
                                            <%# Eval("Location") %>
                                        </div>
                                        
                                        <div class="mb-4">
                                            <h4 class="font-semibold text-gray-800 mb-2">Specialties:</h4>
                                            <div class="flex flex-wrap gap-2">
                                                <asp:Repeater ID="SpecialtiesRepeater" runat="server" DataSource='<%# Eval("Specialties") %>'>
                                                    <ItemTemplate>
                                                        <span class="px-3 py-1 bg-orange-100 text-orange-800 text-sm rounded-full"><%# Container.DataItem %></span>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        
                                        <div class="mb-4">
                                            <h4 class="font-semibold text-gray-800 mb-2">Delivery Options:</h4>
                                            <div class="flex flex-wrap gap-2">
                                                <asp:Repeater ID="DeliveryRepeater" runat="server" DataSource='<%# Eval("Delivery") %>'>
                                                    <ItemTemplate>
                                                        <span class='<%# GetDeliveryClass(Container.DataItem.ToString()) %>'><%# Container.DataItem %></span>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Label ID="NoDeliveryLabel" runat="server" 
                                                    Visible='<%# ((System.Collections.IList)Eval("Delivery")).Count == 0 %>'
                                                    Text="Dine-in Only" 
                                                    CssClass="px-3 py-1 bg-gray-100 text-gray-800 text-sm rounded-full" />
                                            </div>
                                        </div>
                                        
                                        <div class="flex justify-between items-center pt-4 border-t border-gray-200">
                                            <span class="text-lg font-bold text-gray-900"><%# Eval("PriceRange") %></span>
                                            <asp:LinkButton ID="ViewDetailsButton" runat="server" 
                                                CommandName="ViewDetails" 
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="bg-gradient-to-r from-orange-500 to-red-500 text-white px-4 py-2 rounded-lg font-medium hover:from-orange-600 hover:to-red-600 transition-all duration-300 transform hover:scale-105">
                                                View Details
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>

                <!-- Reviews Content -->
                <asp:Panel ID="ReviewsPanel" runat="server" Visible="false">
                    <div class="mb-8">
                        <h2 class="text-2xl font-bold text-gray-900 mb-4">Foodie Reviews & Ratings</h2>
                        <div class="flex space-x-4">
                            <asp:LinkButton ID="AllReviewsButton" runat="server" 
                                CssClass="px-4 py-2 bg-orange-500 text-white rounded-lg"
                                CommandArgument="all" OnClick="ReviewFilterButton_Click">All Reviews</asp:LinkButton>
                            <asp:LinkButton ID="PositiveReviewsButton" runat="server" 
                                CssClass="px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50"
                                CommandArgument="positive" OnClick="ReviewFilterButton_Click">Positive</asp:LinkButton>
                            <asp:LinkButton ID="CriticalReviewsButton" runat="server" 
                                CssClass="px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50"
                                CommandArgument="negative" OnClick="ReviewFilterButton_Click">Critical</asp:LinkButton>
                        </div>
                    </div>

                    <div class="space-y-6">
                        <asp:Repeater ID="ReviewsRepeater" runat="server">
                            <ItemTemplate>
                                <div class="bg-white rounded-xl shadow-md p-6 hover:shadow-lg transition-shadow">
                                    <div class="flex items-start justify-between mb-4">
                                        <div class="flex items-center">
                                            <img src='<%# Eval("UserImage") %>' alt='<%# Eval("UserName") %>' class="w-12 h-12 rounded-full mr-4">
                                            <div>
                                                <h4 class="font-semibold text-gray-900"><%# Eval("UserName") %></h4>
                                                <div class="flex items-center text-sm text-gray-500">
                                                    <span><%# Eval("Date") %></span>
                                                    <span class="mx-2">•</span>
                                                    <span><%# Eval("Platform") %></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="flex items-center">
                                            <%# RenderStarRating((int)Eval("Rating")) %>
                                        </div>
                                    </div>
                                    
                                    <div class="mb-4">
                                        <p class="text-gray-700"><%# Eval("Comment") %></p>
                                    </div>
                                    
                                    <div class="flex items-center justify-between">
                                        <div class="flex items-center text-sm text-gray-500">
                                            <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                                            </svg>
                                            <%# Eval("Likes") %> likes
                                        </div>
                                        <div class="text-sm text-gray-500">
                                            For <%# Eval("CafeName") %>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>

                <!-- Favorites Content -->
                <asp:Panel ID="FavoritesPanel" runat="server" Visible="false">
                    <div class="mb-8">
                        <h2 class="text-2xl font-bold text-gray-900 mb-4">Your Favorite Spots</h2>
                        <p class="text-gray-600">All your saved restaurants in one place for quick access</p>
                    </div>

                    <!-- Add New Cafe Form -->
                    <div class="favorites-form">
                        <div class="favorites-form-header">
                            <h3 class="favorites-form-title">Know a great place? Add it here!</h3>
                            <p class="favorites-form-subtitle">Share your favorite dining spots with the community</p>
                        </div>
                        
                        <div class="favorites-form-content">
                            <div class="form-grid">
                                <div class="form-group">
                                    <label class="form-label">Place Name</label>
                                    <asp:TextBox ID="NewplaceName" runat="server" 
                                        CssClass="form-input"
                                        placeholder="Enter place name"></asp:TextBox>
                                </div>
                                
                                <div class="form-group">
                                    <label class="form-label">Category</label>
                                    <asp:DropDownList ID="Category" runat="server" 
                                        CssClass="form-select">
                                        <asp:ListItem Value="local-favorite">Local Favorite</asp:ListItem>
                                        <asp:ListItem Value="hidden-gem">Hidden Gem</asp:ListItem>
                                        <asp:ListItem Value="branded">Popular Chain</asp:ListItem>
                                        <asp:ListItem Value="foodpanda">Top Delivery</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                <div class="form-group">
                                    <label class="form-label">Location</label>
                                    <asp:TextBox ID="Location" runat="server" 
                                        CssClass="form-input"
                                        placeholder="Enter location"></asp:TextBox>
                                </div>
                                
                                <div class="form-group">
                                    <label class="form-label">Price Range</label>
                                    <asp:TextBox ID="PriceRange" runat="server" 
                                        CssClass="form-input"
                                        placeholder="e.g., Rs 400-800"></asp:TextBox>
                                </div>
                                
                                <div class="form-group form-full-width">
                                    <label class="form-label">Description</label>
                                    <asp:TextBox ID="Description" runat="server" 
                                        TextMode="MultiLine" Rows="3"
                                        CssClass="form-textarea"
                                        placeholder="Describe the place, its specialties, and why you recommend it"></asp:TextBox>
                                </div>
                                
                                <div class="form-group form-full-width">
                                    <label class="form-label">Specialties (comma separated)</label>
                                    <asp:TextBox ID="Specialties" runat="server" 
                                        CssClass="form-input"
                                        placeholder="e.g., Beef Nihari, Biryani, Taka Tak"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Form Legend -->
                            <div class="form-legend">
                                <div class="legend-title">
                                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                                    </svg>
                                    Category Legend
                                </div>
                                <div class="legend-items">
                                    <div class="legend-item">
                                        <div class="legend-badge legend-local-favorite"></div>
                                        <span>Local Favorite - Beloved neighborhood spots</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-badge legend-hidden-gem"></div>
                                        <span>Hidden Gem - Lesser-known treasures</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-badge legend-branded"></div>
                                        <span>Popular Chain - Well-known franchises</span>
                                    </div>
                                    <div class="legend-item">
                                        <div class="legend-badge legend-foodpanda"></div>
                                        <span>Top Delivery - Best delivery options</span>
                                    </div>
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="ClearFormButton" runat="server" Text="Clear Form" 
                                    CssClass="btn-secondary"
                                    OnClick="ClearFormButton_Click" />
                                <asp:Button ID="SubmitCafeButton" runat="server" Text="Submit Cafe" 
                                    CssClass="btn-primary"
                                    OnClick="SubmitCafeButton_Click" />
                            </div>
                        </div>
                    </div>

                    <!-- Success Message -->
                    <asp:Panel ID="SuccessMessagePanel" runat="server" Visible="false" 
                        CssClass="success-message">
                        <svg class="success-icon" fill="currentColor" viewBox="0 0 20 20">
                            <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                        </svg>
                        <span class="success-text">Thank you! Your suggestion has been submitted for review.</span>
                    </asp:Panel>

                    <!-- Favorites Grid -->
                    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                        <asp:Repeater ID="FavoritesRepeater" runat="server" OnItemCommand="FavoritesRepeater_ItemCommand">
                            <ItemTemplate>
                                <div class="bg-white rounded-2xl shadow-xl overflow-hidden hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2">
                                    <div class="relative">
                                        <img src='<%# Eval("Image") %>' alt='<%# Eval("Name") %>' class="w-full h-48 object-cover">
                                        <asp:LinkButton ID="RemoveFavoriteButton" runat="server" 
                                            CommandName="RemoveFavorite" 
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="absolute top-4 right-4 p-2 rounded-full transition-all duration-300 bg-red-500 text-white">
                                            <svg class="w-5 h-5" fill="currentColor" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                                            </svg>
                                        </asp:LinkButton>
                                        <div class="absolute bottom-4 left-4">
                                            <span class='<%# GetCategoryClass(Eval("Category").ToString()) %>'><%# GetCategoryText(Eval("Category").ToString()) %></span>
                                        </div>
                                    </div>
                                    
                                    <div class="p-6">
                                        <div class="flex justify-between items-start mb-3">
                                            <h3 class="text-xl font-bold text-gray-900"><%# Eval("Name") %></h3>
                                            <%# RenderStarRating((double)Eval("Rating")) %>
                                        </div>
                                        
                                        <p class="text-gray-600 mb-4 line-clamp-2"><%# Eval("Description") %></p>
                                        
                                        <div class="flex items-center text-sm text-gray-500 mb-4">
                                            <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                            </svg>
                                            <%# Eval("Location") %>
                                        </div>
                                        
                                        <div class="flex justify-between items-center pt-4 border-t border-gray-200">
                                            <span class="text-lg font-bold text-gray-900"><%# Eval("PriceRange") %></span>
                                            <asp:LinkButton ID="ViewDetailsButton" runat="server" 
                                                CommandName="ViewDetails" 
                                                CommandArgument='<%# Eval("Id") %>'
                                                CssClass="bg-gradient-to-r from-orange-500 to-red-500 text-white px-4 py-2 rounded-lg font-medium hover:from-orange-600 hover:to-red-600 transition-all duration-300 transform hover:scale-105">
                                                View Details
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <asp:Panel ID="EmptyFavoritesPanel" runat="server" CssClass="col-span-full text-center py-12">
                            <div class="bg-gray-100 rounded-2xl p-8 max-w-md mx-auto">
                                <svg class="w-16 h-16 text-gray-400 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                                </svg>
                                <h3 class="text-lg font-medium text-gray-900 mb-2">No favorites yet</h3>
                                <p class="text-gray-600">Start exploring restaurants and click the heart icon to save your favorites!</p>
                                <p class="text-sm text-gray-500 mt-4">Or add a new place using the form above!</p>
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>

            <!-- Footer -->
            <footer class="bg-gray-900 text-white py-12 mt-16">
                <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
                        <div>
                            <div class="flex items-center mb-4">
                                <div class="bg-gradient-to-r from-orange-500 to-red-500 p-2 rounded-full mr-3">
                                    <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                    </svg>
                                </div>
                                <h3 class="text-xl font-bold">Hyderabad Food Guide</h3>
                            </div>
                            <p class="text-gray-400">Your comprehensive guide to the best food experiences in Hyderabad, Pakistan. Discover, review, and share your favorite dining spots.</p>
                        </div>
                        
                        <div>
                            <h4 class="text-lg font-semibold mb-4">Explore</h4>
                            <ul class="space-y-2">
                                <li><a href="#" class="text-gray-400 hover:text-white transition-colors">Restaurant Guide</a></li>
                                <li><a href="#" class="text-gray-400 hover:text-white transition-colors">Food Reviews</a></li>
                                <li><a href="#" class="text-gray-400 hover:text-white transition-colors">Delivery Options</a></li>
                                <li><a href="#" class="text-gray-400 hover:text-white transition-colors">Submit a Review</a></li>
                            </ul>
                        </div>
                        
                        <div>
                            <h4 class="text-lg font-semibold mb-4">Connect With Foodies</h4>
                            <div class="flex space-x-4">
                                <!-- Social media links -->
                            </div>
                        </div>
                    </div>
                    
                    <div class="border-t border-gray-800 mt-8 pt-8 text-center text-gray-400">
                        <p>&copy; 2024 Hyderabad Food Guide. All rights reserved. Your trusted companion for discovering the best food in Hyderabad, Pakistan.</p>
                    </div>
                </div>
            </footer>
        </asp:Panel>

        <!-- Modal for cafe details -->
        <asp:Panel ID="CafeModalPanel" runat="server" CssClass="hidden fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
            <div class="bg-white rounded-2xl max-w-2xl w-full max-h-screen overflow-y-auto">
                <asp:Literal ID="ModalContentLiteral" runat="server" />
            </div>
        </asp:Panel>
    </form>
</body>
</html>