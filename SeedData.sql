-- SQLite
-- Insertar categorías
INSERT INTO Categories (Id, CategoryName) VALUES (1, 'Desserts');
INSERT INTO Categories (Id, CategoryName) VALUES (2, 'Main Course');
INSERT INTO Categories (Id, CategoryName) VALUES (3, 'Salads');

-- Insertar recetas
INSERT INTO Recipes (Id, CreatedDate, Title, Description, PreparationTime, CookingTime, DifficultyLevel, PictureUrl, CategoryId)
VALUES (1, CURRENT_TIMESTAMP, 'Chocolate Cake', 'Delicious chocolate cake with a rich flavor', 30, 60, 'Medium', 'chocolate_cake.jpg', 1);

INSERT INTO Recipes (Id, CreatedDate, Title, Description, PreparationTime, CookingTime, DifficultyLevel, PictureUrl, CategoryId)
VALUES (2, CURRENT_TIMESTAMP, 'Grilled Chicken', 'Juicy grilled chicken with herbs', 15, 45, 'Easy', 'grilled_chicken.jpg', 2);

-- Insertar ingredientes
INSERT INTO Ingredients (Id, IngredientName, Quantity, RecipeId) VALUES (1, 'Flour', '2 cups', 1);
INSERT INTO Ingredients (Id, IngredientName, Quantity, RecipeId) VALUES (2, 'Sugar', '1 cup', 1);
INSERT INTO Ingredients (Id, IngredientName, Quantity, RecipeId) VALUES (3, 'Eggs', '3', 1);
INSERT INTO Ingredients (Id, IngredientName, Quantity, RecipeId) VALUES (4, 'Chicken breast', '4 pieces', 2);
INSERT INTO Ingredients (Id, IngredientName, Quantity, RecipeId) VALUES (5, 'Olive oil', '2 tbsp', 2);

-- Insertar instrucciones
INSERT INTO Instructions (Id, StepNumber, Description, RecipeId) VALUES (1, 1, 'Preheat the oven to 350°F (175°C)', 1);
INSERT INTO Instructions (Id, StepNumber, Description, RecipeId) VALUES (2, 2, 'Mix flour and sugar together', 1);
INSERT INTO Instructions (Id, StepNumber, Description, RecipeId) VALUES (3, 3, 'Add eggs and beat until smooth', 1);
INSERT INTO Instructions (Id, StepNumber, Description, RecipeId) VALUES (4, 1, 'Season chicken with herbs', 2);
INSERT INTO Instructions (Id, StepNumber, Description, RecipeId) VALUES (5, 2, 'Grill chicken for 15 minutes', 2);
