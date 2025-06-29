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
