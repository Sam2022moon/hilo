/*
 * Project:	    A-05 : HI-LO
 * Author:	    Hoang Phuc Tran - 8789102
 * Date:		November 2, 2022
 * Description: This file contains a C# backend of the Hi-Lo Game.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace A05_HiloGame
{
    /*
     * CLASS NAME:  Default
     * PURPOSE : This class is used to implement the post-backs for the Hi-Lo Game
     */
    public partial class Default : System.Web.UI.Page
    {
        const int stateName = 0;
        const int stateMax = 1;
        const int stateGuess = 2;
        const int stateFinal = 3;

        // Contains the user's name
        string name;

        // Maximum and Minimum value
        int min;
        int max;

        // Store the result
        int result;

        /*  -- Method Header Comment
        Name	: getState
        Purpose : Get the current state 
        Inputs	: NONE
        Outputs	: NONE
        Returns	: NONE
        */
        private int getState()
        {
            int stateCurrent = stateName;

            if (ViewState["name"] == null)
            {
                stateCurrent = stateName;
            }
            else if (ViewState["maximumNumber"] == null)
            {
                stateCurrent = stateMax;

            }
            else if (ViewState["win"] == null)
            {
                stateCurrent = stateGuess;
            }
            else
            {
                stateCurrent = stateFinal;
            }
            return stateCurrent;
        }

        /*  -- Method Header Comment
        Name	: IsAllDigits
        Purpose : Check the digits in a string
        Inputs	: a string      s
        Outputs	: NONE
        Returns	: NONE
        */
        private bool IsAllDigits(string s) => s.All(char.IsDigit);

        /*  -- Method Header Comment
        Name	: IsWhiteSpace
        Purpose : Check the whitespace in the name
        Inputs	: a string      input
        Outputs	: NONE
        Returns	: NONE
        */
        private bool IsWhiteSpace(string s) => s.Any(char.IsWhiteSpace);


        /*  -- Method Header Comment
        Name	: checkNumber
        Purpose : Check the valid number
        Inputs	:a string      input
        Outputs	: NONE
        Returns	: NONE
        */
        private bool checkNumber(string input)
        {
            return Int32.TryParse(input, out int number);
        }


        /*  -- Method Header Comment
        Name	: Page_Load
        Purpose : Run the page load
        Inputs	: object sender, EventArgs e
        Outputs	: NONE
        Returns	: NONE
        */
        protected void LoadThePage(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            submission.Text = "Submit";
            inputTxt.Visible = true;

            // Check if the user visits for the first time
            if (IsPostBack)
            {
                validator.ErrorMessage = "";

                // Determine the state of the game
                switch (getState())
                {
                    // Get the user's name
                    case stateName:
                        if (!IsWhiteSpace(inputTxt.Text))
                        {
                            ViewState["name"] = inputTxt.Text;
                        }
                        else
                        {
                            validator.ErrorMessage = "Error!! Please enter a non-blank name.";
                        }
                        break;
                    // Get the maximum value
                    case stateMax:
                        if (IsWhiteSpace(inputTxt.Text))
                        {
                            validator.ErrorMessage = "Error!! Your input must contain only a number!";
                        }
                        else if (!IsAllDigits(inputTxt.Text))
                        {
                            validator.ErrorMessage = "Error!! Your input must contain only a number!";
                        }
                        else if (Int32.Parse(inputTxt.Text.Trim()) > 1 && checkNumber(inputTxt.Text))
                        {
                            int answer;
                            int maximumNumber;

                            maximumNumber = Int32.Parse(inputTxt.Text.Trim());
                            ViewState["maximumNumber"] = maximumNumber;

                            // Generate a random number
                            answer = (new Random(DateTime.Now.Millisecond)).Next(maximumNumber) + 1;
                            ViewState["result"] = answer;

                            ViewState["min"] = 1;
                            ViewState["max"] = maximumNumber;
                        }
                        else
                        {
                            validator.ErrorMessage = "Error!! Please enter a valid max number that is larger than one.";
                        }
                        break;
                    // Get a guess number from the user
                    case stateGuess:
                        if (!IsAllDigits(inputTxt.Text))
                        {
                            validator.ErrorMessage = "Error!! Your input must contain only a number!";
                        }
                        else if (checkNumber(inputTxt.Text))
                        {
                            int guess;
                            guess = Int32.Parse(inputTxt.Text.Trim());

                            // Check if the user's answer is correct or not
                            if ((int)ViewState["result"] == guess)
                            {
                                ViewState["win"] = true;
                            }
                            // Check the min and max value
                            else if (guess <= (int)ViewState["max"] && guess >= (int)ViewState["min"])
                            {
                                if (guess > (int)ViewState["result"])
                                {
                                    ViewState["max"] = guess - 1;
                                }
                                else
                                {
                                    ViewState["min"] = guess + 1;
                                }
                            }
                            else
                            {
                                validator.ErrorMessage = "Error!! Out of range";
                            }
                        }
                        else
                        {
                            validator.ErrorMessage = "Please enter a valid number!";
                        }
                        break;
                    // IF the game is completed, we reset the values
                    case stateFinal:
                        ViewState["maximumNumber"] = null;
                        ViewState["win"] = null;
                        ViewState["min"] = null;
                        ViewState["result"] = null;
                        ViewState["max"] = null;
                        break;
                }
            }

            inputTxt.Focus();
            inputTxt.Text = "";
            form1.Style["background-color"] = "#FFA550";

            // Get values from the current state
            switch (getState())
            {
                // Prompt name
                case stateName:
                    showTxt.Text = "Hi! please enter your username for game!";
                    break;
                // Get name
                case stateMax:
                    name = (string)ViewState["name"];
                    showTxt.Text = string.Format("Hi {0}, Please enter the max number for the game: ", name);
                    break;
                // Prompt user to guess
                case stateGuess:
                    max = (int)ViewState["max"];
                    name = (string)ViewState["name"];
                    min = (int)ViewState["min"];
                    result = (int)ViewState["result"];
                    showTxt.Text = string.Format("{0}, Please make a guess {1} and {2}: ", name, min, max);
                    break;
                case stateFinal:    // the user got the correct result, change the background and offer to play again/
                    result = (int)ViewState["result"];
                    showTxt.Text = string.Format("Congratulations {0} Your answer {1} is correct!", name, result);
                    submission.Text = "Try Again";
                    inputTxt.Visible = false;
                    submission.Focus();
                    form1.Style["background-color"] = "#FFA550";
                    break;
            }

        }


    }
}