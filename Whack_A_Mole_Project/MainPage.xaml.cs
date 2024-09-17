using System.Timers;

namespace Whack_A_Mole_Project;

public partial class MainPage : ContentPage
{
    //keep track of moles whacked
    int score;
    //checks if user has selected 4x4 or 5x5 grid
    int gridCheck;
    //timer setup
    int maxTime = 30;//max time is 30 seconds
    int countSpeed = 1000;//time goes by 1 second fast
    System.Timers.Timer Countdown;
    //random number setup
    Random randomNum;
    public MainPage()
	{
		InitializeComponent();
        randomNum = new Random();
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        //timer setup
        Countdown = new System.Timers.Timer();
        Countdown.Interval = countSpeed;
        Countdown.Elapsed += countElapsed;
    }

    private void countElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        Dispatcher.Dispatch( () => {TimerTick();} );
    }

    private void TimerTick()
    {
        //updates the timer
        int tick;
        tick = Convert.ToInt32(TimeLimit.Text);
        tick--;
        TimeLimit.Text = tick.ToString();
        //when the time reaches 0, game ends
        if(tick == 0)
        {
            endGame();
        }
    }

    private void MoleWhacked(object sender, EventArgs e) //event for when mole is tapped
	{
        int rowPosition;
        int columnPosition;
        //if the user selected a 5x5 grid and sets the next mole position
        if (gridCheck == 0)
        {
            rowPosition = randomNum.Next(0, 5);
            columnPosition = randomNum.Next(0, 5);
        }
        //if the user selected a 4x4 grid
        else
        {
            rowPosition = randomNum.Next(0, 4);
            columnPosition = randomNum.Next(0, 4);
        }
        //updates the score by 1 point
        score = Convert.ToInt32(ScoreUI.Text);
		score++;
		ScoreUI.Text = score.ToString();
        //moves the mole to the next position
        ((ImageButton)sender).SetValue(Grid.RowProperty, rowPosition);
        ((ImageButton)sender).SetValue(Grid.ColumnProperty, columnPosition);
    }


    private void StartGame(object sender, EventArgs e) //when start button is tapped
    {
        switch(StartButton.Text)
        {
            case "Start":
            {
                //changes the start button to a quit button and hides 4x4/5x5 button
                StartButton.Text = "Quit";
                StartButton.BackgroundColor = Colors.Red;
                GridButton.IsVisible = false;

                //checks what grid type the user selected
                if (gridCheck == 0)
                {
                    GameGrid5x5.IsVisible = true;
                    GameGrid4x4.IsVisible = false;
                }
                else
                {
                    GameGrid4x4.IsVisible = true;
                    GameGrid5x5.IsVisible = false;
                }
                //resets the score
                ScoreUI.Text = "0";
                score = Convert.ToInt32(ScoreUI.Text);
                score = 0;
                ScoreUI.Text = score.ToString();

                //displays the time limit at the top of the screen
                //also resets the time limit
                TimeLimit.IsVisible = true;
                TimeLimit.Text = maxTime.ToString();
                Countdown.Start();

                //hides the previous score and text
                PSdisplay.IsVisible = false;
                break;
            }
            //when the user taps the quit button
            case "Quit":
            {
                //asks the user is they are sure they want to quit
                Question.IsVisible = true;
                YesBtn.IsVisible = true;
                NopeBtn.IsVisible = true;
                StartButton.IsVisible = false;
                break;
            }
        }
    }

    private void endGame()//if the user hits the yes button or time limit hits 0
    {
        //resets the start button
        StartButton.Text = "Start";
        StartButton.BackgroundColor = Colors.SeaGreen;
        GridButton.IsVisible = true;

        //hides all grids
        GameGrid5x5.IsVisible = false;
        GameGrid4x4.IsVisible = false;
        ScoreUI.Text = "Select Grid Size";

        //hides and stops time limit
        TimeLimit.IsVisible = false;
        Countdown.Stop();
        //displays the previous score achieved
        PS.Text = score.ToString();
        PSdisplay.IsVisible = true;
    }

    private void GridChange(object sender, EventArgs e)//when the user hits the 5x5/4x4 button
    {
        if (gridCheck == 0)//if the grid button displays 5x5
        {
            //rename button to 4x4
            GridButton.Text = "4x4";
            gridCheck++;
        }
        else//if the grid button displays 4x4
        {
            //rename button to 5x5
            GridButton.Text = "5x5";
            gridCheck--;
        }
    }

    private void Yes(object sender, EventArgs e)//if the user hits the yes button
    {
        endGame();//game ends
        removeQuitDisplay();
    }

    private void Nope(object sender, EventArgs e)//if the user hits the no button
    {
        removeQuitDisplay();
    }

    private void removeQuitDisplay()//hides the yes/no buttons and displays the start/quit button
    {
        Question.IsVisible = false;
        YesBtn.IsVisible = false;
        NopeBtn.IsVisible = false;
        StartButton.IsVisible = true;
    }
}
