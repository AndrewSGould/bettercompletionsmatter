using Ardalis.SmartEnum;

namespace Tavis.Models;

public sealed class GenreList : SmartEnum<GenreList> {
  public static readonly GenreList None = new GenreList(0, "None");
  public static readonly GenreList Action = new GenreList(1, "Action");
  public static readonly GenreList Sports = new GenreList(2, "Sports");
  public static readonly GenreList Football = new GenreList(3, "Football");
  public static readonly GenreList TPS = new GenreList(4, "Third Person Shooter");
  public static readonly GenreList ActionHorror = new GenreList(5, "Action Horror");
  public static readonly GenreList ActionAdventure = new GenreList(6, "Action-Adventure");
  public static readonly GenreList ARPG = new GenreList(7, "Action-RPG");
  public static readonly GenreList RP = new GenreList(8, "Role Playing");
  public static readonly GenreList HackAndSlash = new GenreList(9, "Hack & Slash");
  public static readonly GenreList Aerial = new GenreList(10, "Aerial");
  public static readonly GenreList VehicularCombat = new GenreList(11, "Vehicular Combat");
  public static readonly GenreList AmericanFootball = new GenreList(12, "American Football");
  public static readonly GenreList ArcadeRacing = new GenreList(13, "Arcade Racing");
  public static readonly GenreList Automobile = new GenreList(14, "Automobile");
  public static readonly GenreList AustralianFootball = new GenreList(15, "Australian Football");
  public static readonly GenreList Baseball = new GenreList(16, "Baseball");
  public static readonly GenreList Basketball = new GenreList(17, "Basketball");
  public static readonly GenreList FPS = new GenreList(18, "First Person Shooter");
  public static readonly GenreList BR = new GenreList(19, "Battle Royale");
  public static readonly GenreList BeatEmUp = new GenreList(20, "Beat 'em up");
  public static readonly GenreList Bowling = new GenreList(21, "Bowling");
  public static readonly GenreList Boxing = new GenreList(22, "Boxing");
  public static readonly GenreList Bull = new GenreList(23, "Bull Sports");
  public static readonly GenreList CardAndBoard = new GenreList(24, "Card & Board");
  public static readonly GenreList Casino = new GenreList(25, "Casino");
  public static readonly GenreList CCG = new GenreList(26, "Collectable Card Game");
  public static readonly GenreList Collection = new GenreList(27, "Collection");
  public static readonly GenreList Adventure = new GenreList(28, "Adventure");
  public static readonly GenreList PointAndClick = new GenreList(29, "Point & Click");
  public static readonly GenreList Cricket = new GenreList(30, "Cricket");
  public static readonly GenreList Cue = new GenreList(31, "Cue Sports");
  public static readonly GenreList Platformer = new GenreList(32, "Platformer");
  public static readonly GenreList Cycling = new GenreList(33, "Cycling");
  public static readonly GenreList Dance = new GenreList(34, "Dance");
  public static readonly GenreList Darts = new GenreList(35, "Darts");
  public static readonly GenreList Dodgeball = new GenreList(36, "Dodgeball");
  public static readonly GenreList OpenWorld = new GenreList(37, "Open World");
  public static readonly GenreList DungeonCrawler = new GenreList(38, "Dungeon Crawler");
  public static readonly GenreList EducationalTrivia = new GenreList(39, "Educational & Trivia");
  public static readonly GenreList Party = new GenreList(40, "Party");
  public static readonly GenreList Equestrian = new GenreList(41, "Equestrian Sports");
  public static readonly GenreList Fighting = new GenreList(42, "Fighting");
  public static readonly GenreList Fishing = new GenreList(43, "Fishing");
  public static readonly GenreList Golf = new GenreList(44, "Golf");
  public static readonly GenreList Handball = new GenreList(45, "Handball");
  public static readonly GenreList Simulation = new GenreList(46, "Simulation");
  public static readonly GenreList Health = new GenreList(47, "Health & Fitness");
  public static readonly GenreList Hockey = new GenreList(48, "Hockey");
  public static readonly GenreList Hunting = new GenreList(49, "Hunting");
  public static readonly GenreList Management = new GenreList(50, "Management");
  public static readonly GenreList Mech = new GenreList(51, "Mech");
  public static readonly GenreList Metroidvania = new GenreList(52, "Metroidvania");
  public static readonly GenreList MMA = new GenreList(53, "Mixed Martial Arts");
  public static readonly GenreList MMO = new GenreList(54, "MMO");
  public static readonly GenreList MOBA = new GenreList(55, "MOBA");
  public static readonly GenreList Motocross = new GenreList(56, "Motocross");
  public static readonly GenreList OnRails = new GenreList(57, "On Rails");
  public static readonly GenreList Music = new GenreList(58, "Music");
  public static readonly GenreList Naval = new GenreList(59, "Naval");
  public static readonly GenreList Survival = new GenreList(60, "Survival");
  public static readonly GenreList Paintball = new GenreList(61, "Paintball");
  public static readonly GenreList Pinball = new GenreList(62, "Pinball");
  public static readonly GenreList Puzzle = new GenreList(63, "Puzzle");
  public static readonly GenreList Strategy = new GenreList(64, "Strategy");
  public static readonly GenreList RealTime = new GenreList(65, "Real Time");
  public static readonly GenreList Roguelite = new GenreList(66, "Roguelite");
  public static readonly GenreList Rugby = new GenreList(67, "Rugby");
  public static readonly GenreList RunAndGun = new GenreList(68, "Run & Gun");
  public static readonly GenreList Sandbox = new GenreList(69, "Sandbox");
  public static readonly GenreList Shmup = new GenreList(70, "Shoot 'em up");
  public static readonly GenreList SimRacing = new GenreList(71, "Simulation Racing");
  public static readonly GenreList Skateboarding = new GenreList(72, "Skateboarding");
  public static readonly GenreList Skiing = new GenreList(73, "Skiing");
  public static readonly GenreList Snowboarding = new GenreList(74, "Snowboarding");
  public static readonly GenreList Stealth = new GenreList(75, "Stealth");
  public static readonly GenreList SurvivalHorror = new GenreList(76, "Survival Horror");
  public static readonly GenreList Tennis = new GenreList(77, "Tennis");
  public static readonly GenreList TowerDefence = new GenreList(78, "Tower Defence");
  public static readonly GenreList VN = new GenreList(79, "Visual Novel");
  public static readonly GenreList Volleyball = new GenreList(80, "Volleyball");
  public static readonly GenreList Wrestling = new GenreList(81, "Wrestling");
  public static readonly GenreList TurnBased = new GenreList(82, "Turn Based");

  private GenreList(int id, string name) : base(name, id)
  {
  }
}
