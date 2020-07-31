using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingSequence
{

    /// <summary>
    /// Fields
    /// </summary>

    //the text used to provide information during the training mode
    private TextMeshProUGUI trainingText;
    private const string TRAINING_TEXT_OBJ = "Training text";


    //finite state machine for training mode
    private FiniteStateMachine<TrainingSequence> trainingMachine;


    /// <summary>
    /// Functions
    /// </summary>


    public void Setup()
    {
        trainingText = GameObject.Find(TRAINING_TEXT_OBJ).GetComponent<TextMeshProUGUI>();
        trainingMachine = new FiniteStateMachine<TrainingSequence>(this);
        trainingMachine.TransitionTo<TrainingIntro>();
    }


    /// <summary>
    /// The state machine must have a Tick() so that it will transition between states.
    /// </summary>
    public void Tick()
    {
        trainingMachine.Tick();
    }


    protected void SetTrainingText(string newText)
    {
        trainingText.SetText(newText);
    }


    /// <summary>
    /// Classes
    /// </summary>


    protected class TrainingIntro : FiniteStateMachine<TrainingSequence>.State
    {
        private const string firstStatement = "Welcome to Ridiculously Lethal Laser Swords!";
        private const string secondStatement = "You are a magical space wizard, equipped with a magical space sword.";
        private const string thirdStatement = "Nothing can go wrong.";

        private List<string> statements = new List<string>() { firstStatement, secondStatement, thirdStatement };
        private int statementIndex = 0;

        private const string pushToCont = "<size=16>\n\n(Press Enter to continue.)</size>";

        public override void OnEnter()
        {
            Context.SetTrainingText(statements[statementIndex] + pushToCont);
            Services.Events.Register<KeypressEvent>(ProgressText);
        }

        private void ProgressText(global::Event e)
        {
            statementIndex++;

            if (statementIndex < statements.Count) Context.SetTrainingText(statements[statementIndex] + pushToCont);
            else TransitionTo<MoveSword>();
        }


        public override void OnExit()
        {
            Services.Events.Unregister<KeypressEvent>(ProgressText);
        }
    }


    protected class MoveSword : FiniteStateMachine<TrainingSequence>.State
    {

        /// <summary>
        /// Statements
        /// </summary>
        private const string firstStatement = "Let's learn to move your sword.";
        private const string secondStatement = "Use these keys to move your hands up, down, left, and right: W, S, A, and D.";
        private const string thirdStatement = "Use these keys to swing your sword: I, K, J, and L.";
        private const string fourthStatement = "Great. You're ready to face some opposition!";
        private const string tryNow = "\n\nTry it now.";


        private List<string> statements = new List<string>() { firstStatement, secondStatement, thirdStatement, fourthStatement };
        private int statementIndex = 0;

        private const string pushToCont = "<size=16>\n\n(Press Enter to continue.)</size>";


        /// <summary>
        /// Used to track whether player has tried all movements
        /// </summary>

        private List<InputManager.Directions> directionsPressed = new List<InputManager.Directions>();
        private List<KeyboardInput.CardinalDirections> cardinalDirsPressed = new List<KeyboardInput.CardinalDirections>();
        private bool allHandDirections = false;
        private bool allWristRotations = false;



        public override void OnEnter()
        {
            Context.SetTrainingText(statements[statementIndex] + pushToCont);
            Services.Events.Register<KeypressEvent>(ProgressText);
            Services.Events.Register<KeyDirectionEvent>(GetDirectionPress);
            Services.Events.Register<CardinalDirectionEvent>(GetRotationPress);
        }


        private void GetDirectionPress(global::Event e)
        {
            KeyDirectionEvent dirEvent = e as KeyDirectionEvent;

            if (!directionsPressed.Contains(dirEvent.direction)) directionsPressed.Add(dirEvent.direction);

            //if the player has pressed all four cardinal directions, the tutorial can proceed
            if (directionsPressed.Contains(InputManager.Directions.Up))
            {
                if (directionsPressed.Contains(InputManager.Directions.Down))
                {
                    if (directionsPressed.Contains(InputManager.Directions.Left))
                    {
                        if (directionsPressed.Contains(InputManager.Directions.Right) && !allHandDirections)
                        {
                            statementIndex++;
                            Services.Events.Unregister<KeyDirectionEvent>(GetDirectionPress); //stop listening for direction presses, to avoid progressing the text accidentally
                            Services.Events.Fire(new KeypressEvent(InputManager.UsefulKeys.Enter)); //send a fake keypress to get ProgressText to listen
                            allHandDirections = true; //only process one input event to avoid progressing the text too far this frame
                        }
                    }
                }
            }
        }


        private void GetRotationPress(global::Event e)
        {
            CardinalDirectionEvent dirEvent = e as CardinalDirectionEvent;

            if (!cardinalDirsPressed.Contains(dirEvent.cardinalDir)) cardinalDirsPressed.Add(dirEvent.cardinalDir);

            
            //if the player has rotated their wrists all four ways, the tutorial can proceed
            if (cardinalDirsPressed.Contains(KeyboardInput.CardinalDirections.North))
            {
                if (cardinalDirsPressed.Contains(KeyboardInput.CardinalDirections.South))
                {
                    if (cardinalDirsPressed.Contains(KeyboardInput.CardinalDirections.West))
                    {
                        if (cardinalDirsPressed.Contains(KeyboardInput.CardinalDirections.East) && !allWristRotations)
                        {
                            statementIndex++;
                            Services.Events.Unregister<CardinalDirectionEvent>(GetRotationPress); //stop listening for rotation presses, to avoid progressing the text in future frames
                            Services.Events.Fire(new KeypressEvent(InputManager.UsefulKeys.Enter)); //send a fake keypress to get ProgressText to listen
                            allWristRotations = true;  //only process one input event to avoid progressing the text too far this frame
                        }
                    }
                }
            }
        }

        private void ProgressText(global::Event e)
        {
            //the first statement is introductory; go on after enter is pressed. Same for the fourth, final statement
            if (statements[statementIndex] == firstStatement || statements[statementIndex] == fourthStatement) statementIndex++;

            //the second and third statements handle themselves in GetDirectionPress and GetRotationPress, respectively

            if (statementIndex < statements.Count)
            {
                if (statements[statementIndex] == secondStatement || statements[statementIndex] == thirdStatement)
                {
                    Context.SetTrainingText(statements[statementIndex] + tryNow);
                }
                else Context.SetTrainingText(statements[statementIndex] + pushToCont);
            }
            else TransitionTo<BlockAndStrike>();
        }


        public override void OnExit()
        {
            Services.Events.Unregister<KeypressEvent>(ProgressText);
            
        }


        protected class BlockAndStrike : FiniteStateMachine<TrainingSequence>.State
        {
            /// <summary>
            /// Fields
            /// </summary>
            

            //statements
            private const string firstStatement = "You can use your sword to parry your opponent's strikes.";
            private const string secondStatement = "When you tell it you are ready, the training dummy will take a swing at you.";
            private const string thirdStatement = "Don't worry, the training dummy can't hurt you if you miss.";
            private const string fourthStatement = "Just move your sword to block the training dummy's swing.";
            private const string fifthStatement = "Now, press the Space bar to tell the training dummy to swing.";


            private List<string> statements = new List<string>() { firstStatement, secondStatement, thirdStatement, fourthStatement, fifthStatement };
            private int statementIndex = 0;

            private const string pushToCont = "<size=16>\n\n(Press Enter to continue.)</size>";


            //the training dummy now becomes active
            private GameObject opponent;
            private const string OPPONENT_OBJ = "Player 2";


            /// <summary>
            /// Functions
            /// </summary>


            public override void OnEnter()
            {
                Context.SetTrainingText(statements[statementIndex] + pushToCont);
                Services.Events.Register<KeypressEvent>(ProgressText);
            }


            private void ProgressText(global::Event e)
            {
                KeypressEvent keyEvent = e as KeypressEvent;
                if (keyEvent.key != InputManager.UsefulKeys.Enter) return; //only respond to Enter; the training dummy should respond to Space

                //don't progress past the fifth statement until the player blocks successfully
                if (statements[statementIndex] != fifthStatement) statementIndex++;

                if (statementIndex < statements.Count)
                {
                    if (statements[statementIndex] == fifthStatement)
                    {
                        Context.SetTrainingText(statements[statementIndex]);
                        ActivateOpponent();
                    }
                    else Context.SetTrainingText(statements[statementIndex] + pushToCont);
                }
                else TransitionTo<BlockAndStrike>();
            }


            private void ActivateOpponent()
            {
                Services.Swordfighters.ChangeOpponentType<Opponent.TrainingDummyBehavior>();
            }


            private void CheckForParry(global::Event e)
            {

            }


            public override void OnExit()
            {
                Services.Events.Unregister<KeypressEvent>(ProgressText);
            }
        }
    }
}
