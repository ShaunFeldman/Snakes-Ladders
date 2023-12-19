// Author: Shaun Feldman
// File Name: Main.cs
// Project Name: Snakes_1
// Creation Date: November 3, 2021
// Modified Date: November 4, 2021
// Description:  2 player snakes and ladders game. The following extra additions have been made: Translation (piece movement), Animation (dice rolling), Space coordinate translation, background music (menu music, game music), dice rolling sound effect, sequence of spaces translation, 1 player mode (you vs computer)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation2D;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Snakes_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Variable to create random numbers
        static Random rng = new Random();

        //Variables to store the random numbers made as well as which player will start dependent on the random num
        int randomNum;
        int playerSelectorStart;

        //Variables to store the screen height and width
        int screenHeight;
        int screenWidth;

        //Variables to contain the turn counter and the previous turn counter
        int turnCounter = 1;
        int prevTurnCounter = 0;

        //Variables to containe the starting positions for the green piece and the blue piece
        int starterBlueX = 55;
        int starterGreenX = 55;
        int starterBlueY = 645;
        int starterGreenY = 670;

        //Variables to store the dices number (i is the one used to access arrays)
        int i;
        int diceNum;

        //Variables to store how big one space is for its x value and y value
        int oneSpaceX = 75;
        int oneSpaceY = 70;

        //Variables to store the x position for the right and left sides of the board
        int rightSideUp = 730;
        int leftSideUp = 55;

        //Arrays to contain the positions of both x and y for when I want to activate or deactivate the player moving along a snake or a ladder
        int[] snakeLaddersXActivation = { 205, 580, 430, 55, 730, 280, 655, 130 };
        int[] snakeLaddersYActivation = { 645, 505, 295, 155, 85, 575, 225, 15 };
        int[] snakeLaddersXDeactivation = { 280, 430, 580, 655, 130, 205 };
        int[] snakeLaddersYDeactivation = { 505, 85, 155, 15, 575, 435, 295 };

        //Variables to contain things I want to happen like the y distance between the blue and green piece, the direction I want the player to move, and the speeds of the pieces
        int blueGreenYDist = 25;
        int dirX = 1;
        int speedRightLeft = 3;
        int speedUp = 2;

        //bool variables to contain when I want to start something or stop it aka which screens I want to appear and when
        bool startScrn = true;
        bool gameOver = false;
        bool game = false;
        bool gameMode = false;
        bool statsScreen = false;

        //Bools to state when I want to output the dice, whose turn it is, and when I want which piece to go right, left, up, and down as well as the previous directions of both the blue and green piece
        bool diceOutput = false;
        bool playerOneTurn = true;
        bool rightBlue = true;
        bool leftBlue = false;
        bool upBlue = false;
        bool rightGreen = true;
        bool leftGreen = false;
        bool upGreen = false;
        bool prevDirLeftGreen = false;
        bool prevDirRightGreen = true;
        bool prevDirRightBlue = true;
        bool prevDirLeftBlue = false;

        //Bools to contain if it's one player playing and if the ai should in that case be activated
        bool one = false;
        bool ai = false;

        //Bools to be activated when the green or blue pieces go on the ladder or snake
        bool snakeLadder = false;
        bool ladder1B = false;
        bool ladder1G = false;
        bool ladder2B = false;
        bool ladder2G = false;
        bool ladder3B = false;
        bool ladder3G = false;
        bool ladder4B = false;
        bool ladder4G = false;
        bool ladder5B = false;
        bool ladder5G = false;
        bool ladder6B = false;
        bool ladder6G = false;
        bool ladder7B = false;
        bool ladder7G = false;
        bool snake1B = false;
        bool snake1G = false;
        bool snake2B = false;
        bool snake2G = false;
        bool snake3B = false;
        bool snake3G = false;
        bool snake4B = false;
        bool snake4G = false;
        bool snake5B = false;
        bool snake5G = false;
        bool snake6B = false;
        bool snake6G = false;
        bool snake7B = false;
        bool snake7G = false;

        //SpriteFonts for the titles, win texts, the instructions, statistics etc
        SpriteFont titleFont;
        SpriteFont starting;
        SpriteFont playerOneW;
        SpriteFont playerTwoW;
        SpriteFont rollDiceInstr;
        SpriteFont statsInfo;

        //Locations for all the text SpriteFont above
        Vector2 titleLoc = new Vector2(0, 100);
        Vector2 startingLoc = new Vector2(0, 500);
        Vector2 playerWLoc = new Vector2(0, 20);
        Vector2 diceRollLoc = new Vector2(1000, 150);
        Vector2 rollDiceInstrLoc = new Vector2(830, 70);
        Vector2 statsInfoLoc = new Vector2(20, 10);

        //The string message which will show through the SpriteFont variables above
        string titleTxt = "Snakes & Ladders";
        string startingTxt = "PRESS A TO PLAY";
        string playerOneTxt = "PLAYER 1 TURN";
        string playerTwoTxt = "PLAYER 2 TURN";
        string rollDiceInstrTxt = "PRESS X TO ROLL THE DICE";
        string statsInfoTxt = "";

        //Texture2D to containe the images in the game, like the gameboard, menu images, dice, player pieces, etc
        Texture2D backgroundMenu;
        Texture2D gameBoard;
        Texture2D diceRoll;
        Texture2D [] diceSide = new Texture2D [6];
        Texture2D bluePiece;
        Texture2D greenPiece;
        Texture2D onePlayer;
        Texture2D twoPlayer;

        //Rectangle to later contain the location of the images above
        Rectangle backgroundMenuRec;
        Rectangle gameBoardRec;
        Rectangle diceSideRec;
        Rectangle bluePieceRec;
        Rectangle greenPieceRec;
        Rectangle onePlayerRec;
        Rectangle twoPlayerRec;

        //Contains the state of the mouse
        MouseState ms;

        //Contains the state of the keyboard
        KeyboardState kb;
        KeyboardState prevKb;

        //An animation variable to later create an animation
        Animation diceRollAni;

        //Statistics Variables
        int[] stats = new int [5];
        double averageNumRolls;
        double winPercentageOne;
        double winPercentageTwo;

        //Sound effect of the dice rolling
        SoundEffect diceRollingSound;

        //Music to spice up the game, make it interesting and a bit more intense
        Song gameIntroMusic;
        Song gameMusic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Sets the width and height of the screen and applys the changes
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth = 1400;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Variables to contain the height and width of the screen
            screenHeight = _graphics.GraphicsDevice.Viewport.Height;
            screenWidth = _graphics.GraphicsDevice.Viewport.Width;

            //SpriteFonts to load in all the text
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            starting = Content.Load<SpriteFont>("Fonts/StartingInfo");
            playerOneW = Content.Load<SpriteFont>("Fonts/PlayerInfo");
            playerTwoW = Content.Load<SpriteFont>("Fonts/PlayerInfo");
            rollDiceInstr = Content.Load<SpriteFont>("Fonts/RollDice");
            statsInfo = Content.Load<SpriteFont>("Fonts/StatsInfo");

            //Texture2D to load in all the images
            backgroundMenu = Content.Load<Texture2D>("Images/Sprites/BG-Menu");
            gameBoard = Content.Load<Texture2D>("Images/Sprites/snakesandladders");
            diceRoll = Content.Load<Texture2D>("Images/Sprites/DiceRollingAni");
            diceSide[0] = Content.Load<Texture2D>("Images/Sprites/DiceSides1");
            diceSide[1] = Content.Load<Texture2D>("Images/Sprites/DiceSides2");
            diceSide[2] = Content.Load<Texture2D>("Images/Sprites/DiceSides3");
            diceSide[3] = Content.Load<Texture2D>("Images/Sprites/DiceSides4");
            diceSide[4] = Content.Load<Texture2D>("Images/Sprites/DiceSides5");
            diceSide[5] = Content.Load<Texture2D>("Images/Sprites/DiceSides6");
            bluePiece = Content.Load<Texture2D>("Images/Sprites/PieceBlue");
            greenPiece = Content.Load<Texture2D>("Images/Sprites/PieceBlue");
            onePlayer = Content.Load<Texture2D>("Images/Sprites/1PlayerImg");
            twoPlayer = Content.Load<Texture2D>("Images/Sprites/2PlayerImg");

            //Rectangles to store the locations of all the images
            backgroundMenuRec = new Rectangle(0, 0, screenWidth, screenHeight);
            gameBoardRec = new Rectangle(0, 0, (screenWidth / 2) + 100, screenHeight);
            diceSideRec = new Rectangle(1000, 150, 200, 200);
            bluePieceRec = new Rectangle(55, 645, 20, 40);
            greenPieceRec = new Rectangle(55, 670, 20, 40);
            onePlayerRec = new Rectangle(150, 200, 400, 400);
            twoPlayerRec = new Rectangle(900, 200, 400, 400);

            //Creating the animation of the dice rolling
            diceRollAni = new Animation(diceRoll, 8, 1, 8, 0, Animation.NO_IDLE, 2, 3, diceRollLoc, 2f, false);

            //Loads all the sounds into the game/ Music and sound effects and sets their volume and if they are repeating or not
            diceRollingSound = Content.Load<SoundEffect>("Audio/Sounds/roll");
            gameIntroMusic = Content.Load<Song>("Audio/Music/16. Oogway Acsends - Hans Zimmer (Kung Fu Panda Soundtrack)");
            gameMusic = Content.Load<Song>("Audio/Music/01. Hero - Hans Zimmer (Kung Fu Panda Soundtrack)");
            SoundEffect.MasterVolume = 0.8f;
            MediaPlayer.Volume = 0.35f;
            MediaPlayer.IsRepeating = true;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Centers the location of the title and the starting text as well as positions the playing win text for later
            titleLoc.X = (screenWidth / 2) - ((int)titleFont.MeasureString(titleTxt).X / 2);
            startingLoc.X = (screenWidth / 2) - ((int)starting.MeasureString(startingTxt).X / 2);
            playerWLoc.X = screenWidth - 450;

            //sets the previous keyboard state to equal what it is now
            prevKb = kb;

            //gets the mouse state and keyboard state
            ms = Mouse.GetState();
            kb = Keyboard.GetState();

            //Outputs the start screen if true, with the option for the user to start the game, plays the game intro music
            if (startScrn == true)
            {
                //sets the music to game intro music and begins playing it
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(gameIntroMusic);
                }

                //If the user presses A then go to the game mode screen
                if (kb.IsKeyDown(Keys.A))
                {
                    //Sets the starting screen to end and the game mode screen showing they can play solo or duo mode
                    startScrn = false;
                    gameMode = true;
                    startingTxt = "Choose Game Mode\n\n     Solo --- Duo";
                    startingLoc.Y = 40;
                }
            }

            //If the game mode screen it present and the user presses 1 player make the ai activate as the 2nd plyer, if the user presses 2 players go to game screen
            if (gameMode == true)
            {
                //If the user presses on the mouse button then follow which mode they picked
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    //If the users mouse is in the 1 player rectangle switch on the ai, make it one player mode, switch off the game mode screen and switch on the game screen
                    if (onePlayerRec.Contains(ms.Position))
                    {
                        //One player mode is now true and ai is on, then switch from the game mode screen to the game screen
                        one = true;
                        ai = true;
                        gameMode = false;
                        game = true;
                    }

                    //if the users mouse is in the 2 player rectangle then switch off game mode, select a random number from 1-2 make game true and the turn counter will now equal the random number created
                    if (twoPlayerRec.Contains(ms.Position))
                    {
                        //switch from game mode to game and select a random number from 1-2 and make turn counter equal it
                        gameMode = false;
                        playerSelectorStart = rng.Next(1, 3);
                        game = true;
                        turnCounter = playerSelectorStart;
                    }

                    //Stop the mediaplayer from playing music
                    MediaPlayer.Stop();
                }
            }

            //If the game screen it true check if the user going to roll the dice, move the user in the correct position, check if they're on a snake or ladder, and check if they reach the end point
            if (game == true && gameOver == false)
            {
                //Play the game music
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(gameMusic);
                }

                //if the turn counter is odd then it's player ones turn, else if it isn't then it's player twos turn and the ai activates
                if (turnCounter % 2 == 1)
                {
                    playerOneTurn = true;
                    ai = false;
                }
                else
                {
                    playerOneTurn = false;
                    ai = true;
                }

                //If single player is true then if it's player ones turn make them have to roll the dice by pressing x, if players two turn make it automatically roll
                if (one == true)
                {
                    //if player ones turn is true call the dice if they press x else, if player twos turn is true make it automatically roll
                    if (playerOneTurn == true)
                    {
                        //if the user press x and the turn counter doesn't equal its previous number and ai is false and the keyboard isn't equal to its previous state then call the dice method
                        if (kb.IsKeyDown(Keys.X)&& turnCounter != prevTurnCounter && ai == false && prevKb != kb)
                        {
                            //Calls the dice roll method
                            Dice();
                        }
                    }
                    else
                    {
                        //if the ai is true and turn counter isn't equal to previou turn counter call the dice rolling method
                        if (turnCounter != prevTurnCounter && ai == true)
                        {
                            //Calls the dice roll method
                            Dice();
                        }
                    }
                }
                else
                {
                    //if the user presses x and the turn counter doesn't equal the previous turn counter and the keyboard state doesnt equal the previous keyboard state call the dice method
                    if (kb.IsKeyDown(Keys.X) && turnCounter != prevTurnCounter && prevKb != kb)
                    {
                        //Calls the dice roll method
                        Dice();
                    }
                }

                //If the dice output is true and the animation ends then do the following: if its player ones turn call the everything method with player ones piece info, if it's player twos call the everything method with player twos piece info, check if the player reaches the end point (the 100 block) and if the players dice number reaches 0 then move to the next players turn
                if (diceOutput == true && diceRollAni.isAnimating == false)
                {
                    //If it's player ones turn call the everything method will player ones information, else call the everything with player twos info
                    if (playerOneTurn == true)
                    {
                        //Calls the everything method
                        Everything(ref bluePieceRec, ref starterBlueX, ref starterBlueY, ref upBlue, ref rightBlue, ref leftBlue, ref prevDirRightBlue, ref prevDirLeftBlue, ref ladder1B, ref ladder2B, ref ladder3B, ref ladder4B, ref ladder5B, ref ladder6B, ref ladder7B, ref snake1B, ref snake2B, ref snake3B, ref snake4B, ref snake5B, ref snake6B, ref snake7B);
                    } 
                    else
                    {
                        //Calls the everything method
                        Everything(ref greenPieceRec, ref starterGreenX, ref starterGreenY, ref upGreen, ref rightGreen, ref leftGreen, ref prevDirRightGreen, ref prevDirLeftGreen, ref ladder1G, ref ladder2G, ref ladder3G, ref ladder4G, ref ladder5G, ref ladder6G, ref ladder7G, ref snake1G, ref snake2G, ref snake3G, ref snake4G, ref snake5G, ref snake6G, ref snake7G);
                    }

                    //If the blue piece reaches the position of the 100 square or the green piece reaches there reset the game and add one win to the player who reached there and change the text to say that player one
                    if ((bluePieceRec.X == leftSideUp && bluePieceRec.Y == snakeLaddersYDeactivation[3]) || (greenPieceRec.X == leftSideUp && greenPieceRec.Y == (snakeLaddersYDeactivation[3] + blueGreenYDist)))
                    {
                        //If it's player ones turn then add one to player ones win count, else add one to player twos win count
                        if (playerOneTurn == true)
                        {
                            stats[0] = stats[0] + 1;
                        }
                        else
                        {
                            stats[1] = stats[1] + 1;
                        }

                        //Calculates the average number of rolls, the win percentage of player one and two
                        averageNumRolls = Math.Round((double)(stats[2] + stats[3]) / (stats[0] + stats[1]));
                        winPercentageOne = Math.Round((double)stats[0] / (stats[0] + stats[1]) * 100);
                        winPercentageTwo = Math.Round((double)stats[1] / (stats[0] + stats[1]) * 100);

                        //Sets the game to end and stops outputing the dice image as well as stops showing the dice instructions
                        gameOver = true;
                        diceOutput = false;
                        rollDiceInstrTxt = "";

                        //sets all the directions for both players to false so there is no more movement on the board from the two pieces
                        leftBlue = false;
                        rightBlue = false;
                        upBlue = false;
                        leftGreen = false;
                        rightGreen = false;
                        upGreen = false;

                        //If it is player ones turn say player one wins, else say player two wins + press a to see statistics for both situations
                        if (playerOneTurn == true)
                        {
                            //Sets the text to player one wins and see statistics
                            playerOneTxt = "Player One Wins\n-------------------\n      Press A\nTo See Statistics";
                        }
                        else
                        {
                            //Sets the text to player two wins and see statistics
                            playerTwoTxt = "Player Two Wins\n-------------------\n      Press A\nTo See Statistics";
                        }  
                    }

                    //If the random number reaches 0 and there is no snake or ladder movement takin place and the game is not over stop outputing the dice and add one to the turn counter
                    if (randomNum == 0 && snakeLadder == false && gameOver == false)
                    {
                        //makes the dices output false an goes to the next turn by adding one to the turn counter
                        diceOutput = false;
                        turnCounter += 1;
                    }
                }
            }

            //If the user presses A and the game is over set stats screen to pop up and show 
            if (kb.IsKeyDown(Keys.A) && gameOver == true)
            {
                //Makes the game false and the stats screen true
                game = false;
                statsScreen = true;

                //Outputs the statistics (total games played, wins for both players, win percentage for both players, total number of dice rolls and average number of dice rolls as well as if they press x it'll start a new game but if they press y it'll end the game
                statsInfoTxt = "STATISTICS\n---------------\nYou've Played " + (stats[0] + stats[1]) + " Game(s)\n\nPlayer One Has " + stats[0] + " Win(s)\nPlayer Two Has " + stats[1] + " Win(s)\n\nPlayer One Win Percentage: " + winPercentageOne + "%\nPlayer Two Win Percentage: " + winPercentageTwo + "%\n\nTotal Number Of Dice Rolls: " + (stats[2] + stats[3]) + "\nAverage Number Of Dice Rolls: " + averageNumRolls + " \n\n> PRESS X FOR NEW GAME <\n\n > PRESS Y TO END GAME <";
            }

            //If the statistics screen is true and the keyboard state isn't equal to the previous keyboard state then if the user presses xreset all the variables and restart the game, if they press y make the code stop running
            if (statsScreen == true && kb != prevKb)
            {
                //If the user presses x then reset all the variables required to restart the game
                if (kb.IsKeyDown(Keys.X))
                {
                    //Sets the stats screen and game over to false while game becomes true
                    statsScreen = false;
                    game = true;
                    gameOver = false;

                    //The previous turn counter is set to 0
                    prevTurnCounter = 0;

                    //Sets the blue and green pieces to their original x and y positions and the variables which are used to track their position also to the starting positions
                    starterBlueX = 55;
                    starterGreenX = 55;
                    starterBlueY = 645;
                    starterGreenY = 670;
                    bluePieceRec.X = 55;
                    greenPieceRec.X = 55;
                    bluePieceRec.Y = 645;
                    greenPieceRec.Y = 670;

                    //Sets the player one text to player one turn and the player two text to player two turn and the dice instructions back to press x to roll the dice
                    playerOneTxt = "PLAYER 1 TURN";
                    playerTwoTxt = "PLAYER 2 TURN";
                    rollDiceInstrTxt = "PRESS X TO ROLL THE DICE";

                    //Sets the directions of the two pieces back to going right and the other directions false as well as their previous directions to true only if it contains right
                    rightBlue = true;
                    rightGreen = true;
                    prevDirLeftGreen = false;
                    prevDirRightGreen = true;
                    prevDirRightBlue = true;
                    prevDirLeftBlue = false;
                    diceOutput = false;

                    //Sets the random number to 0
                    randomNum = 0;

                    //Randomly selects a number from 1 to 2 and sets the turn counter to equal that number;
                    playerSelectorStart = rng.Next(1, 3);
                    turnCounter = playerSelectorStart;

                    //Sets the dice number to 0 and the dices output to false as well as the direction of the pieces to positive therefore going right
                    diceNum = 0;
                    diceOutput = false;
                    dirX = 1;
                }

                //If the user presses y end the code
                if (kb.IsKeyDown(Keys.Y))
                {
                    //Ends the code
                    Environment.Exit(0);
                }
            }

            diceRollAni.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightYellow);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            //If the start screen is true draw the background as well as write the title and starting text
            if (startScrn == true)
            {
                //Draws the images and the text
                _spriteBatch.Draw(backgroundMenu, backgroundMenuRec, Color.White * 0.5f);
                _spriteBatch.DrawString(titleFont, titleTxt, titleLoc, Color.MediumPurple);
                _spriteBatch.DrawString(starting, startingTxt, startingLoc, Color.Blue);
            }

            //If game mode is true write the text and draw the one player and two player images
            if (gameMode == true)
            {
                //Writes the instruction text and shows the one and two player images
                _spriteBatch.DrawString(starting, startingTxt, startingLoc, Color.Blue);
                _spriteBatch.Draw(onePlayer, onePlayerRec, Color.White);
                _spriteBatch.Draw(twoPlayer, twoPlayerRec, Color.White);
            }

            //If game is true then draw the board, the dice animation, the dice instructions, the blue and green pieces, as well as the text of whos turn it is and the dice image
            if (game == true)
            {
                //Draws the gameboard, the two pieces, the dice rolling instruction text and the dice animation
                _spriteBatch.Draw(gameBoard, gameBoardRec, Color.White);
                diceRollAni.Draw(_spriteBatch, Color.White, Animation.FLIP_NONE);
                _spriteBatch.DrawString(rollDiceInstr, rollDiceInstrTxt, rollDiceInstrLoc, Color.Goldenrod);
                _spriteBatch.Draw(bluePiece, bluePieceRec, Color.White);
                _spriteBatch.Draw(greenPiece, greenPieceRec, Color.LawnGreen);

                //If player ones turn is true draw the player one turn text, else draw player 2 text
                if (playerOneTurn == true)
                {
                    //Draws the player one turn text
                    _spriteBatch.DrawString(playerOneW, playerOneTxt, playerWLoc, Color.DarkViolet);
                }
                else
                {
                    //Draws the player two turn text
                    _spriteBatch.DrawString(playerTwoW, playerTwoTxt, playerWLoc, Color.DarkViolet);
                }

                //If the dice output is true and it's not animation draw the dice image dependant on the random number created
                if (diceOutput == true && diceRollAni.isAnimating == false)
                {
                    //Draws the particular dice image array corresponding to the random number selected from 1 - 6
                    _spriteBatch.Draw(diceSide[i - 1], diceSideRec, Color.White);
                }
            }

            //If the statistics screen is true draw the statistics text
            if (statsScreen == true)
            {
                //Draws the stats text
                _spriteBatch.DrawString(statsInfo, statsInfoTxt, statsInfoLoc, Color.DarkViolet);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Calculates everything required to move the pieces, checks if they are on a special place like a snake or a ladder and makes them move
        private void Everything(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool upColor, ref bool rightColor, ref bool leftColor, ref bool prevDirRightColor, ref bool prevDirLeftColor, ref bool ladder1C, ref bool ladder2C, ref bool ladder3C, ref bool ladder4C, ref bool ladder5C, ref bool ladder6C, ref bool ladder7C, ref bool snake1C, ref bool snake2C, ref bool snake3C, ref bool snake4C, ref bool snake5C, ref bool snake6C, ref bool snake7C)
        {
            //If the piece is going right and random number doesn't equal 0 then make the direction positive therefore right and call the translation subprogram
            if (rightColor == true && randomNum != 0)
            {
                //sets the direction to positive
                dirX = 1;

                //calls the rightLeft subprogram which will move the piece on whoevers turn it's on in accordance to what the dice rolled 
                RightLeft(ref colorPieceRec, ref starterColorX, ref starterColorY, ref upColor, ref rightColor, ref leftColor, ref prevDirRightColor, ref prevDirLeftColor);
            }

            //If the piece is going up and random number doesnt equal 0 then call the up subprogram
            if (upColor == true && randomNum != 0)
            {
                //Calls the up program which will move the piece on whoevers turn it's on in accordance to what the dice rolled
                Up(ref colorPieceRec, ref starterColorX, ref starterColorY, ref upColor, ref rightColor, ref leftColor, ref prevDirRightColor, ref prevDirLeftColor);
            }

            //If the piece is going left and random number doesn't equal 0 then make the direction negative therefore left and call the translation subprogram
            if (leftColor == true && randomNum != 0)
            {
                //Sets the direction to negative
                dirX = -1;

                //calls the rightLeft subprogram which will move the piece on whoevers turn it's on in accordance to what the dice rolled
                RightLeft(ref colorPieceRec, ref starterColorX, ref starterColorY, ref upColor, ref rightColor, ref leftColor, ref prevDirRightColor, ref prevDirLeftColor);
            }

            //if the players x position is at a certian point and the y is on one of the two points (green of blue since they have different y's but same x's) and random number doesnt equal 0 set snakeLadder to true and ladder 1 is activated by ladder 1 bool becoming true and sets the direction of which the piece was going to false, if not true checks different points and sets those points ladder or snake to true and sets their directions to false dependant on which position was true
            if (colorPieceRec.X == snakeLaddersXActivation[0] && (colorPieceRec.Y == snakeLaddersYActivation[0] || colorPieceRec.Y == snakeLaddersYActivation[0] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 1 true as well as makes the piece no longer move right
                snakeLadder = true;
                ladder1C = true;
                rightColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[1] && (colorPieceRec.Y == snakeLaddersYActivation[0] || colorPieceRec.Y == snakeLaddersYActivation[0] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 2 true as well as makes the piece no longer move right
                snakeLadder = true;
                ladder2C = true;
                rightColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[1] && (colorPieceRec.Y == snakeLaddersYActivation[1] || colorPieceRec.Y == snakeLaddersYActivation[1] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 3 true as well as makes the piece no longer move right
                snakeLadder = true;
                ladder3C = true;
                rightColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[0] && (colorPieceRec.Y == snakeLaddersYActivation[2] || colorPieceRec.Y == snakeLaddersYActivation[2] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 4 true as well as makes the piece no longer move left
                snakeLadder = true;
                ladder4C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[2] && (colorPieceRec.Y == snakeLaddersYActivation[3] || colorPieceRec.Y == snakeLaddersYActivation[3] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 5 true as well as makes the piece no longer move left
                snakeLadder = true;
                ladder5C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[3] && (colorPieceRec.Y == snakeLaddersYActivation[3] || colorPieceRec.Y == snakeLaddersYActivation[3] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 6 true as well as makes the piece no longer move up
                snakeLadder = true;
                ladder6C = true;
                rightColor = false;
                upColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[4] && (colorPieceRec.Y == snakeLaddersYActivation[4]  || colorPieceRec.Y == snakeLaddersYActivation[4] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes ladder 7 true as well as makes the piece no longer move up
                snakeLadder = true;
                ladder7C = true;
                rightColor = false;
                upColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[5] && (colorPieceRec.Y == snakeLaddersYActivation[5] || colorPieceRec.Y == snakeLaddersYActivation[5] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 1 true and makes the piece no longer move left
                snakeLadder = true;
                snake1C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[6] && (colorPieceRec.Y == snakeLaddersYActivation[2] || colorPieceRec.Y == snakeLaddersYActivation[2] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 2 true and makes the piece no longer move left
                snakeLadder = true;
                snake2C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[5] && (colorPieceRec.Y == snakeLaddersYActivation[2] || colorPieceRec.Y == snakeLaddersYActivation[2] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 3 true and makes the piece no longer move left
                snakeLadder = true;
                snake3C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[7] && (colorPieceRec.Y == snakeLaddersYActivation[6] || colorPieceRec.Y == snakeLaddersYActivation[6] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 4 true and makes the piece no longer move right
                snakeLadder = true;
                snake4C = true;
                rightColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[1] && (colorPieceRec.Y == snakeLaddersYActivation[4] || colorPieceRec.Y == snakeLaddersYActivation[4] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 5 true and makes the piece no longer move right
                snakeLadder = true;
                snake5C = true;
                rightColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[2] && (colorPieceRec.Y == snakeLaddersYActivation[7] || colorPieceRec.Y == snakeLaddersYActivation[7] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 6 true and makes the piece no longer move left
                snakeLadder = true;
                snake6C = true;
                leftColor = false;
            }
            else if (colorPieceRec.X == snakeLaddersXActivation[5] && (colorPieceRec.Y == snakeLaddersYActivation[7] || colorPieceRec.Y == snakeLaddersYActivation[7] + blueGreenYDist) && randomNum == 0)
            {
                //Sets the snake ladder to be true and makes snake 7 true and makes the piece no longer move left
                snakeLadder = true;
                snake7C = true;
                leftColor = false;
            }

            //Checks if the ladder or snake subprograms should be called dependant on if they've been activated by checking to see if that specific ladder or snake bool is true
            if (ladder1C == true)
            {
                //Calls the ladder 1 subprogram
                Ladder_1(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder2C == true)
            {
                //Calls the ladder 2 subprogram
                Ladder_2(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder3C == true)
            {
                //Calls the ladder 3 subprogram
                Ladder_3(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder4C == true)
            {
                //Calls the ladder 4 subprogram
                Ladder_4(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder5C == true)
            {
                //Calls the ladder 5 subprogram
                Ladder_5(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder6C == true)
            {
                //Calls the ladder 6 subprogram
                Ladder_6(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (ladder7C == true)
            {
                //Calls the ladder 7 subprogram
                Ladder_7(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake1C == true)
            {
                //Calls the snake 1 subprogram
                Snake_1(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake2C == true)
            {
                //Calls the snake 2 subprogram
                Snake_2(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake3C == true)
            {
                //Calls the snake 3 subprogram
                Snake_3(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake4C == true)
            {
                //Calls the snake 4 subprogram
                Snake_4(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake5C == true)
            {
                //Calls the snake 5 subprogram
                Snake_5(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake6C == true)
            {
                //Calls the snake 6 subprogram
                Snake_6(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
            else if (snake7C == true)
            {
                //Calls the snake 7 subprogram
                Snake_7(ref colorPieceRec, ref starterColorX, ref starterColorY, ref rightColor, ref leftColor, ref upColor, ref prevDirLeftColor, ref prevDirRightColor);
            }
        }

        //Pre: The pieces x position needs to be on the game board, starter color x needs to equal the pieces rec x
        //Post: None
        //Desc: Calculates and translates the correct movement of the piece right and left dependant on its dice roll
        private void RightLeft(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool upColor, ref bool rightColor, ref bool leftColor, ref bool prevDirRightColor, ref bool prevDirLeftColor)
        {
            //Moves the piece x wise right or left dependant on the speed and direction
            colorPieceRec.X += dirX * speedRightLeft;

            //If the pieces x position is equal to what it was before plus one full space then subtract 1 from random number and make the used to be x position variable equal to the now x position
            if (colorPieceRec.X == starterColorX + oneSpaceX)
            {
                //Sets starter color to what it was plus one full space as well as takes away one from the random number
                starterColorX = starterColorX + oneSpaceX;
                randomNum -= 1;
            }

            //If the pieces x position is equal to what it was before minus one full space then subtract 1 from random number and make the used to be x position variable equal to the now x position
            if (colorPieceRec.X == starterColorX - oneSpaceX)
            {
                //Sets starter color to what it was minus one full space as well as takes away one from the random number
                starterColorX = starterColorX - oneSpaceX;
                randomNum -= 1;
            }

            //If the pieces x position hits the right side of the board set up to be true while all the other directions to false and its previous dir left to false while prev dir right true
            if (colorPieceRec.X == rightSideUp)
            {
                //sets up to be true while all the other directions to false and its previous dir left to false while prev dir right true
                upColor = true;
                rightColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }

            //If the pieces x position hits the left side of the board set up to be true while all other directions to false and its previous dir left to true and previous dir right to false
            if (colorPieceRec.X == leftSideUp)
            {
                //sets up to be true while all other directions to false and its previous dir left to true and previous dir right to false
                upColor = true;
                leftColor = false;
                prevDirLeftColor = true;
                prevDirRightColor = false;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter color x needs to equal the pieces rec x
        //Post: None
        //Desc: Calculates and translates the correct movement of the piece moving up dependant on its dice roll
        private void Up(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool upColor, ref bool rightColor, ref bool leftColor, ref bool prevDirRightColor, ref bool prevDirLeftColor)
        {
            //Moves the piece up by the speed number
            colorPieceRec.Y -= speedUp;

            //If the pieces y position is equal to what it was minus one full y space set its used to be y pos to what it is now and make up false and take one from random number
            if (colorPieceRec.Y == starterColorY - oneSpaceY)
            {
                //Sets the starter color to the new y pos and makes up false as well as takes 1 from the random number
                starterColorY = starterColorY - oneSpaceY;
                upColor = false;
                randomNum -= 1;

                //If the pieces x pos is on the right side and the previous direction right is true then make right false and left true
                if (colorPieceRec.X == rightSideUp && prevDirRightColor == true)
                {
                    //makes right false and left true
                    rightColor = false;
                    leftColor = true;
                }

                //If the pieces x pos is on the left side and the previous direction left is true then make left false and right true
                if (colorPieceRec.X == leftSideUp && prevDirLeftColor == true)
                {
                    //makes left false and right true
                    leftColor = false;
                    rightColor = true;
                }
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the first ladder
        private void Ladder_1(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the x and y position of the piece up and left
            colorPieceRec.X -= oneSpaceX / 5;
            colorPieceRec.Y -= oneSpaceY / 5;

            //If the piece hits the left side of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == leftSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[0] || colorPieceRec.Y == snakeLaddersYDeactivation[0] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder1B = false;
                ladder1G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Sets it to go right and not left or up as well as the previous directions to what flase for left and true for right
                rightColor = true;
                leftColor = false;
                upColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the second ladder
        private void Ladder_2(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the x and y position of the piece up and right
            colorPieceRec.X += oneSpaceX / 5;
            colorPieceRec.Y -= oneSpaceY / 5;

            //If the piece hits the right side of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == rightSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[0] || colorPieceRec.Y == snakeLaddersYDeactivation[0] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder2B = false;
                ladder2G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Sets it to go up and not left or right and the previous direction left to false while right to true
                rightColor = false;
                leftColor = false;
                upColor = true;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the third ladder
        private void Ladder_3(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the x and y position of the piece up and left
            colorPieceRec.X -= oneSpaceX / 3;
            colorPieceRec.Y -= oneSpaceY / 2;

            //If the piece hits a certian position of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[0] && (colorPieceRec.Y == snakeLaddersYDeactivation[1] || colorPieceRec.Y == snakeLaddersYDeactivation[1] + blueGreenYDist))
            {
                // Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder3B = false;
                ladder3G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Sets it to go right not left or up and the previous direction left to false and right to true
                rightColor = true;
                leftColor = false;
                upColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the fourth ladder
        private void Ladder_4(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the x and y position of the piece up and right
            colorPieceRec.X += oneSpaceX / 5;
            colorPieceRec.Y -= (oneSpaceY * 2) / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[0] && (colorPieceRec.Y == snakeLaddersYDeactivation[2] || colorPieceRec.Y == snakeLaddersYDeactivation[2] + blueGreenYDist))
            {
                // Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder4B = false;
                ladder4G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Sets it to go left and not right or up as well as the previous direction left to true while right to false
                leftColor = true;
                rightColor = false;
                upColor = false;
                prevDirLeftColor = true;
                prevDirRightColor = false;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the fifth ladder
        private void Ladder_5(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece up
            colorPieceRec.Y -= oneSpaceY / 10;

            //If the piece hits a certian position of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[1] && (colorPieceRec.Y == snakeLaddersYDeactivation[1] || colorPieceRec.Y == snakeLaddersYDeactivation[1] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder5B = false;
                ladder5G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Sets the piece to move right not left or up and previous direction left to true while left to false
                leftColor = false;
                rightColor = true;
                upColor = false;
                prevDirLeftColor = true;
                prevDirRightColor = false;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the sixth ladder
        private void Ladder_6(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece up
            colorPieceRec.Y -= oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == leftSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[3] || colorPieceRec.Y == snakeLaddersYDeactivation[3] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the ladder bools false and sets the starting colors position to the x and y of the current pieces postion
                snakeLadder = false;
                ladder6B = false;
                ladder6G = false;
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece up the seventh ladder
        private void Ladder_7(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece up
            colorPieceRec.Y -= oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the ladder movement
            if (colorPieceRec.X == rightSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[3] || colorPieceRec.Y == snakeLaddersYDeactivation[3] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the ladder bools false
                snakeLadder = false;
                ladder7B = false;
                ladder7G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move left not right or up and the previous direction left to false and right to true
                leftColor = true;
                rightColor = false;
                upColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the first snake
        private void Snake_1(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the x position of the piece right
            colorPieceRec.X += oneSpaceX / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[2] && (colorPieceRec.Y == snakeLaddersYDeactivation[4] || colorPieceRec.Y == snakeLaddersYDeactivation[4] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake1B = false;
                snake1G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorY = colorPieceRec.Y;
                starterColorX = colorPieceRec.X;

                //Makes the piece move left not right or up and sets the previous direction left to false and right to true
                leftColor = true;
                rightColor = false;
                upColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the second snake
        private void Snake_2(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down
            colorPieceRec.Y += oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[3] && (colorPieceRec.Y == snakeLaddersYDeactivation[0] || colorPieceRec.Y == snakeLaddersYDeactivation[0] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake2B = false;
                snake2G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move right not left or up and sets the previous direction right to false and left to true
                rightColor = true;
                leftColor = false;
                upColor = false;
                prevDirLeftColor = true;
                prevDirRightColor = false;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the third snake
        private void Snake_3(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down and the x position left
            colorPieceRec.X -= (oneSpaceX * 3) / 5;
            colorPieceRec.Y += (oneSpaceY * 2) / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == leftSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[5] || colorPieceRec.Y == snakeLaddersYDeactivation[5] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake3B = false;
                snake3G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move up and not left or right and sets the previous direction right to false and left to true
                upColor = true;
                leftColor = false;
                rightColor = false;
                prevDirRightColor = false;
                prevDirLeftColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the fourth snake
        private void Snake_4(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down
            colorPieceRec.Y += oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[4] && (colorPieceRec.Y == snakeLaddersYDeactivation[0] || colorPieceRec.Y == snakeLaddersYDeactivation[0] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake4B = false;
                snake4G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move right not left or up and sets the previous direction left to true and right to false
                upColor = false;
                rightColor = true;
                leftColor = false;
                prevDirLeftColor = true;
                prevDirRightColor = false;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the fifth snake
        private void Snake_5(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down and the x position left
            colorPieceRec.X -= oneSpaceX;
            colorPieceRec.Y += oneSpaceY * 7 / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[5] && (colorPieceRec.Y == snakeLaddersYDeactivation[4] || colorPieceRec.Y == snakeLaddersYDeactivation[4] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake5B = false;
                snake5G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move left not right or up and sets the previous direction left to false and right to true
                upColor = false;
                leftColor = true;
                rightColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the sixth snake
        private void Snake_6(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down and the x position right
            colorPieceRec.X += oneSpaceX / 5;
            colorPieceRec.Y += oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == rightSideUp && (colorPieceRec.Y == snakeLaddersYDeactivation[6] || colorPieceRec.Y == snakeLaddersYDeactivation[6] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake6B = false;
                snake6G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move left not right or up and sets the previous direction left to false and right to true
                upColor = false;
                leftColor = true;
                rightColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: The pieces x and y position needs to be on the game board, starter colors x and y need to equal the pieces rec's x and y
        //Post: None
        //Desc: Translates the piece down the seventh snake
        private void Snake_7(ref Rectangle colorPieceRec, ref int starterColorX, ref int starterColorY, ref bool rightColor, ref bool leftColor, ref bool upColor, ref bool prevDirLeftColor, ref bool prevDirRightColor)
        {
            //Moves the y position of the piece down and the x position left
            colorPieceRec.X -= oneSpaceX / 5;
            colorPieceRec.Y += oneSpaceY / 5;

            //If the piece hits a certian position of the board and the y position is one of the two end the snake movement
            if (colorPieceRec.X == snakeLaddersXDeactivation[4] && (colorPieceRec.Y == snakeLaddersYDeactivation[2] || colorPieceRec.Y == snakeLaddersYDeactivation[2] + blueGreenYDist))
            {
                //Sets snakeLadder bool to false and makes the snake bools false
                snakeLadder = false;
                snake7B = false;
                snake7G = false;

                //Sets the starting colors position to the x and y of the current pieces postion
                starterColorX = colorPieceRec.X;
                starterColorY = colorPieceRec.Y;

                //Makes the piece move left not right or up and sets the previous direction left to false and right to true
                upColor = false;
                leftColor = true;
                rightColor = false;
                prevDirLeftColor = false;
                prevDirRightColor = true;
            }
        }

        //Pre: None
        //Post: None
        //Desc: creates the dice rolling sound effect and begins the dice rolling animation as well as randomly chooses the next number from 1 - 6 and sets i to equal the random number, makes dice num equal the random number and previous turn counter becomes the turn counter right now and outputs the dice plus adds 1 to the roll counter on whevers player turn it is
        private void Dice()
        {
            //Plays the dice rolling sound effect
            diceRollingSound.CreateInstance().Play();

            //begins the animation for the dice rolling and selects a number that the dice will roll
            diceRollAni.isAnimating = true;
            randomNum = rng.Next(1, 7);

            //Sets the dice number to the random number selected
            diceNum = randomNum;

            //Makes the previous turn counter equal to the turn counter
            prevTurnCounter = turnCounter;

            //Outputs the dice and sets i to equal the random number
            diceOutput = true;
            i = randomNum;

            //If it's player ones turn add one to player ones dice rolls else add one to player twos dice rolls
            if (playerOneTurn == true)
            {
                //Adds one to player ones dice rolls
                stats[2] += 1;
            }
            else
            {
                //Adds one to the player twos dice rolls
                stats[3] += 1;
            }
        }
    }
}