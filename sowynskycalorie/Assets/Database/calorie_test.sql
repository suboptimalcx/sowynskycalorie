CREATE DATABASE  IF NOT EXISTS `sowynsky_calorie` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `sowynsky_calorie`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: sowynsky_calorie
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `meals`
--

DROP TABLE IF EXISTS `meals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meals` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` char(100) DEFAULT NULL,
  `description` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meals`
--

LOCK TABLES `meals` WRITE;
/*!40000 ALTER TABLE `meals` DISABLE KEYS */;
INSERT INTO `meals` VALUES 
(1,'dinner','cool dinner'),
(2,'breakfast','god breakfast'),
(3,'protein bomb','super combo'),
(4,'veggie delight','light and healthy vegetarian combo'),
(5,'muscle builder','high-protein meal for muscle gain'),
(6,'classic breakfast','balanced traditional morning meal'),
(7,'energy booster','carb-focused meal to fuel the day');
/*!40000 ALTER TABLE `meals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meals_products`
--

DROP TABLE IF EXISTS `meals_products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meals_products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `mealID` int NOT NULL,
  `productID` int NOT NULL,
  `grams` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `mealID` (`mealID`),
  KEY `productID` (`productID`),
  CONSTRAINT `meals_products_ibfk_1` FOREIGN KEY (`mealID`) REFERENCES `meals` (`id`),
  CONSTRAINT `meals_products_ibfk_2` FOREIGN KEY (`productID`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meals_products`
--

LOCK TABLES `meals_products` WRITE;
/*!40000 ALTER TABLE `meals_products` DISABLE KEYS */;
INSERT INTO `meals_products` VALUES 
(1,1,1,100),
(2,1,2,100),
(3,2,3,50),
(4,2,4,75),
(5,3,1,200),
(6,3,5,100),
(7,4,8,100),
(8,4,12,150),
(9,4,15,100),
(10,5,1,200),
(11,5,5,50),
(12,5,16,30),
(13,6,4,100),
(14,6,10,200),
(15,6,20,100),
(16,7,2,150),
(17,7,7,100),
(18,7,13,100);
/*!40000 ALTER TABLE `meals_products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meals_ratings`
--

DROP TABLE IF EXISTS `meals_ratings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meals_ratings` (
  `id` int NOT NULL AUTO_INCREMENT,
  `userID` int NOT NULL,
  `mealID` int NOT NULL,
  `meal_rating` enum('1','2','3','4','5','6','7','8','9','10') DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `mealID` (`mealID`,`userID`),
  KEY `userID` (`userID`),
  CONSTRAINT `meals_ratings_ibfk_1` FOREIGN KEY (`mealID`) REFERENCES `meals` (`id`),
  CONSTRAINT `meals_ratings_ibfk_2` FOREIGN KEY (`userID`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meals_ratings`
--

LOCK TABLES `meals_ratings` WRITE;
/*!40000 ALTER TABLE `meals_ratings` DISABLE KEYS */;
/*!40000 ALTER TABLE `meals_ratings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` char(30) NOT NULL,
  `calories` int NOT NULL,
  `protein` decimal(5,2) NOT NULL,
  `carbohydrates` decimal(5,2) NOT NULL,
  `fat` decimal(5,2) NOT NULL,
  `category` enum('meat','fish','dairy','eggs','grains','bread','rice_and_pasta','vegetables','fruits','nuts_and_seeds','legumes','fats_and_oils','sweets','snacks','beverages','spices','sauces','frozen_foods','ready_meals','other') NOT NULL DEFAULT 'other',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES
(1,'Chicken breast',165,31.00,0.00,3.60,'meat'),
(2,'Brown rice',216,5.00,44.80,1.80,'rice_and_pasta'),
(3,'Avocado',160,2.00,8.50,14.70,'fruits'),
(4,'Egg',68,5.50,0.60,4.80,'eggs'),
(5,'Protein bar',200,20.00,22.00,6.50,'snacks'),
(6,'Almonds',579,21.15,21.55,49.93,'nuts_and_seeds'),
(7,'Banana',89,1.09,22.84,0.33,'fruits'),
(8,'Broccoli',34,2.82,6.64,0.37,'vegetables'),
(9,'Salmon',208,20.42,0.00,13.42,'fish'),
(10,'Whole milk',61,3.15,4.80,3.25,'dairy'),
(11,'Olive oil',884,0.00,0.00,100.00,'fats_and_oils'),
(12,'Sweet potato',86,1.57,20.12,0.05,'vegetables'),
(13,'Apple',52,0.26,13.81,0.17,'fruits'),
(14,'Beef steak',271,25.00,0.00,19.00,'meat'),
(15,'Tofu',76,8.00,1.90,4.80,'legumes'),
(16,'Peanut butter',588,25.00,20.00,50.00,'nuts_and_seeds'),
(17,'Yogurt',59,10.00,3.60,0.40,'dairy'),
(18,'Cheddar cheese',402,25.00,1.30,33.00,'dairy'),
(19,'Lentils',116,9.02,20.13,0.38,'legumes'),
(20,'Oatmeal',68,2.40,12.00,1.40,'grains');
(21, 'Turkey breast', 135, 30.00, 0.00, 1.00, 'meat'),
(22, 'White rice', 205, 4.20, 44.50, 0.40, 'rice_and_pasta'),
(23, 'Blueberries', 57, 0.70, 14.50, 0.30, 'fruits'),
(24, 'Carrots', 41, 0.90, 9.60, 0.20, 'vegetables'),
(25, 'Greek yogurt', 59, 10.00, 3.60, 0.40, 'dairy'),
(26, 'Cottage cheese', 98, 11.10, 3.40, 4.30, 'dairy'),
(27, 'Cucumber', 16, 0.70, 3.60, 0.10, 'vegetables'),
(28, 'Orange', 62, 1.20, 15.40, 0.20, 'fruits'),
(29, 'Tilapia', 129, 26.20, 0.00, 2.70, 'fish'),
(30, 'White bread', 266, 8.00, 49.00, 3.30, 'bread'),
(31, 'Pasta', 131, 5.00, 25.00, 1.10, 'rice_and_pasta'),
(32, 'Hummus', 166, 7.90, 14.30, 8.60, 'legumes'),
(33, 'Cashews', 553, 18.00, 30.00, 44.00, 'nuts_and_seeds'),
(34, 'Spinach', 23, 2.90, 3.60, 0.40, 'vegetables'),
(35, 'Cranberries', 46, 0.40, 12.20, 0.10, 'fruits'),
(36, 'Walnuts', 654, 15.00, 14.00, 65.00, 'nuts_and_seeds'),
(37, 'Green peas', 81, 5.40, 14.50, 0.40, 'vegetables'),
(38, 'Bacon', 541, 37.00, 1.40, 42.00, 'meat'),
(39, 'Shrimp', 99, 24.00, 0.20, 0.30, 'fish'),
(40, 'Mushrooms', 22, 3.10, 3.30, 0.30, 'vegetables'),
(41, 'Zucchini', 17, 1.20, 3.10, 0.30, 'vegetables'),
(42, 'Tomato', 18, 0.90, 3.90, 0.20, 'vegetables'),
(43, 'Pineapple', 50, 0.50, 13.10, 0.10, 'fruits'),
(44, 'Strawberries', 32, 0.70, 7.70, 0.30, 'fruits'),
(45, 'Dark chocolate', 546, 4.90, 46.40, 31.30, 'sweets'),
(46, 'Granola bar', 193, 3.10, 29.00, 7.00, 'snacks'),
(47, 'Kale', 49, 4.30, 8.80, 0.90, 'vegetables'),
(48, 'Brussels sprouts', 43, 3.40, 8.90, 0.30, 'vegetables'),
(49, 'Chickpeas', 164, 8.90, 27.40, 2.60, 'legumes'),
(50, 'Pumpkin seeds', 559, 30.00, 10.70, 49.00, 'nuts_and_seeds'),
(51, 'Macaroni and cheese', 310, 10.00, 31.00, 17.00, 'ready_meals'),
(52, 'Mayonnaise', 680, 1.00, 0.00, 75.00, 'sauces'),
(53, 'Ketchup', 112, 1.00, 25.00, 0.20, 'sauces'),
(54, 'Hot sauce', 15, 0.50, 3.00, 0.10, 'spices'),
(55, 'Cereal', 379, 6.90, 83.00, 1.30, 'grains'),
(56, 'Pancake syrup', 260, 0.00, 67.00, 0.00, 'sweets'),
(57, 'Green tea', 0, 0.00, 0.00, 0.00, 'beverages'),
(58, 'Orange juice', 45, 0.70, 10.40, 0.20, 'beverages'),
(59, 'Cola', 140, 0.00, 39.00, 0.00, 'beverages'),
(60, 'Energy drink', 110, 1.00, 29.00, 0.00, 'beverages'),
(61, 'Ice cream', 207, 4.00, 24.00, 11.00, 'frozen_foods'),
(62, 'Frozen pizza', 298, 12.00, 33.00, 13.00, 'frozen_foods'),
(63, 'Burrito', 290, 14.00, 33.00, 12.00, 'ready_meals'),
(64, 'Chicken nuggets', 302, 15.00, 18.00, 20.00, 'frozen_foods'),
(65, 'Pita bread', 275, 9.00, 55.00, 1.20, 'bread'),
(66, 'Bagel', 250, 9.00, 48.00, 1.50, 'bread'),
(67, 'Basmati rice', 210, 4.00, 45.00, 0.50, 'rice_and_pasta'),
(68, 'Quinoa', 120, 4.10, 21.30, 1.90, 'grains'),
(69, 'Raisins', 299, 3.10, 79.30, 0.50, 'fruits'),
(70, 'Dates', 282, 2.50, 75.00, 0.40, 'fruits'),
(71, 'Salsa', 36, 1.50, 7.00, 0.10, 'sauces'),
(72, 'Barbecue sauce', 130, 1.00, 34.00, 0.20, 'sauces'),
(73, 'Mozzarella', 280, 28.00, 3.10, 17.00, 'dairy'),
(74, 'Ricotta cheese', 174, 11.30, 3.80, 13.00, 'dairy'),
(75, 'Butter', 717, 0.85, 0.06, 81.00, 'fats_and_oils'),
(76, 'Margarine', 717, 0.20, 0.00, 80.00, 'fats_and_oils'),
(77, 'Pine nuts', 673, 13.70, 13.10, 68.40, 'nuts_and_seeds'),
(78, 'Hazelnuts', 628, 15.00, 17.00, 61.00, 'nuts_and_seeds'),
(79, 'Sunflower seeds', 584, 21.00, 20.00, 51.00, 'nuts_and_seeds'),
(80, 'Soy milk', 54, 3.30, 6.00, 1.75, 'dairy');

/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products_preferences`
--

DROP TABLE IF EXISTS `products_preferences`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products_preferences` (
  `id` int NOT NULL AUTO_INCREMENT,
  `userID` int NOT NULL,
  `productID` int NOT NULL,
  `preference` enum('like','dislike') DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `unique_user_product` (`userID`,`productID`),
  KEY `productID` (`productID`),
  CONSTRAINT `products_preferences_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `users` (`id`),
  CONSTRAINT `products_preferences_ibfk_2` FOREIGN KEY (`productID`) REFERENCES `products` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products_preferences`
--

LOCK TABLES `products_preferences` WRITE;
/*!40000 ALTER TABLE `products_preferences` DISABLE KEYS */;
/*!40000 ALTER TABLE `products_preferences` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` char(30) NOT NULL,
  `password` char(30) NOT NULL,
  `weight` decimal(5,2) NOT NULL,
  `height` decimal(5,2) NOT NULL,
  `sex` tinyint(1) NOT NULL,
  `activitylvl` enum('sedentary','light','moderate','very','super') DEFAULT NULL,
  `DoB` date NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'anna_nowak','secure123',65.50,170.00,0,'moderate','1990-05-15'),(2,'jan_kowalski','qwerty456',80.25,185.50,1,'very','1985-11-22'),(3,'user123','pass789',70.00,175.00,1,'sedentary','2000-03-10'),(4,'ola_zielinska','olaz1234',60.75,165.00,0,'super','1995-08-30'),(5,'adam_malinowski','adam123',90.00,190.00,1,'light','1978-07-04');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-19 19:07:46
