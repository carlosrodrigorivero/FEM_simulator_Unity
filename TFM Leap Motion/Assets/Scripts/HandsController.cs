using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using System.Diagnostics;

public class HandsController : MonoBehaviour
{
    public bool launchMatlabGraphAnalysis;
    public bool saveDataInDatabaseOption;

    //public variables
    public MotionLeapListener listener;
    public int currentGameNumber;
    public int numberOfSecondsCalibrating;
    public int minimumDistanceExercise2;
    public Animator animator;
    public bool activeGame;
    public GameObject ball;
    public GameObject dirtyBall;
    public Material bubbleMaterial;
    public Material footballBallMaterial;
    public UnityEngine.UI.Text alertText;
    public GameObject fingersIndicatorTextGameObject;
    public GameObject timeExercise4TextGameObject;
    public GameObject cleaningText;
    public Texture orangeAlertTexture;
    public Texture greenAlertTexture;
    public UnityEngine.UI.RawImage alertRawImage;
    public GameObject sliderBarGameObject;
    public int exercise3Height;
    public int clapSpeed;
    public List<string> fingerNouns;
    public float timeExercise4;
    public int distanceFingerToHand;
    public GameObject fistTextGameObject;
    public int currentNumberOfFistsExercise5;
    public int maxNumberOfFistsExercise5;
    public float limitGrabStrengthValue;
    public UnityEngine.UI.InputField ageTextInputfield;
    public UnityEngine.UI.Button startIntroButton;
    public AudioSource clapSound;
    public GameObject clapPanel;
    public GameObject maleGenreButton;
    public GameObject femaleGenreButton;
    public GameObject undefinedGenreButton;
    public GameObject clapAlertText;
    public UnityEngine.UI.RawImage clapAlertRawImage;


    //private variables
    private Vector firstHandPosition;
    private Vector secondHandPosition;
    private Vector firstHandVelocity;
    private Vector secondHandVelocity;
    private float timeExercise1;
    private Vector3 scaleChange;
    private float xHandToBallDistanceFirstHand;
    private float yHandToBallDistanceFirstHand;
    private float xHandToBallDistanceSecondHand;
    private float yHandToBallDistanceSecondHand;
    private float xHandToBallDistanceFirstHandPartialValue;
    private float yHandToBallDistanceFirstHandPartialValue;
    private float xHandToBallDistanceSecondHandPartialValue;
    private float yHandToBallDistanceSecondHandPartialValue;
    private MeshRenderer dirtBallMesh;
    private float ballRadius;
    private bool exercise4StepPassed;
    private int exercise4NumberOfStepsPassed;
    private int currentFinger;
    private float timeExercise4PerFinger;
    private bool fistFirstHandActive;
    private bool fistSecondHandActive;
    private float timeExercise5;
    private int csvRow;
    private int csvColumns;
    private StringBuilder sbExercise1;
    private StringBuilder sbExercise2;
    private StringBuilder sbExercise3;
    private StringBuilder sbExercise4;
    private StringBuilder sbExercise5;
    private StringBuilder sbExercise6;
    private string csvName;
    private float limitTimePerFingerExercise4;
    private float handVelocityExercise3;
    private int age;
    private bool introPassed;
    private string genre;
    private GameObject myEventSystem;
    private float timeExercise6;
    private int numberOfInteractionsFirstHand;
    private float accumulatedVelocityFirstHand;
    private float mediumVelocityFirstHand;
    private int numberOfInteractionsSecondHand;
    private float accumulatedVelocitySecondHand;
    private float mediumVelocitySecondHand;
    private float maximumGrabStrengthFirstHand;
    private float maximumGrabStrengthSecondHand;
    private float minimumGrabStrengthFirstHand;
    private float minimumGrabStrengthSecondHand;
    private float grabStrengthMinumum;
    private int fistNumber;
    private string id;
    private enum typeMatlabGraph {HANDVELOCITY, HANDPALMPOSITION, FINGERSPOSITION, GRABSTRENGTH}
    private string grabStrength1;
    private string grabStrength2;
    private string fingersPosition1_Hand1;
    private string fingersPosition2_Hand1;
    private string fingersPosition3_Hand1;
    private string fingersPosition4_Hand1;
    private string fingersPosition5_Hand1;
    private string fingersPosition1_Hand2;
    private string fingersPosition2_Hand2;
    private string fingersPosition3_Hand2;
    private string fingersPosition4_Hand2;
    private string fingersPosition5_Hand2;
    private string palmPosition1;
    private string palmPosition2;
    private string palmVelocity1;
    private string palmVelocity2;


    void Awake()
    {
        grabStrength1 = "";
        grabStrength2 = "";
        id = System.DateTime.Now.Year + "" + System.DateTime.Now.Month + "" + System.DateTime.Now.Day + "" + System.DateTime.Now.Minute + "" + System.DateTime.Now.Second + "" + System.DateTime.Now.Millisecond;
        UnityEngine.Debug.Log(id);
        grabStrengthMinumum = 0.5f;
        minimumGrabStrengthFirstHand = float.MaxValue;
        minimumGrabStrengthSecondHand = float.MaxValue;
        timeExercise6 = 5;
        Color temp = maleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
        temp.a = 0.5f;
        maleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
        temp = femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
        temp.a = 0.5f;
        femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp; 
        temp = undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color;
        temp.a = 0.5f;
        undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
        myEventSystem = GameObject.Find("EventSystem");
        genre = "";
        handVelocityExercise3 = 15;
        limitTimePerFingerExercise4 = 3;
        csvName = "dataCompilation";
        sbExercise1 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        sbExercise2 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        sbExercise3 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        sbExercise4 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        sbExercise5 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        sbExercise6 = new StringBuilder("First hand speed;Second hand speed;First hand palm position;;;Second hand palm position;;;First hand fingers position;;;;;;;;;;;;;;;Second hand fingers position;;;;;;;;;;;;;;;First hand grab strength;Second hand grab strength");
        limitGrabStrengthValue = 1;
        timeExercise5 = 5;
        maxNumberOfFistsExercise5 = 1;
        distanceFingerToHand = 100;
        exercise4StepPassed = true;
        fingerNouns.Add("PULGAR");
        fingerNouns.Add("ÍNDICE");
        fingerNouns.Add("MEDIO");
        fingerNouns.Add("ANULAR");
        fingerNouns.Add("MEÑIQUE");
        scaleChange = new Vector3(0,0,0);
        currentGameNumber = 0;
        minimumDistanceExercise2 = 400;
        exercise3Height = 280;
        clapSpeed = 20;
        numberOfSecondsCalibrating = 5;
        listener = new MotionLeapListener();
        activeGame = false;
        dirtBallMesh = dirtyBall.GetComponent<MeshRenderer>();
        Mesh mesh = ball.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        ballRadius = Vector3.Distance(transform.position, vertices[0]);
        //alertText.text = "Coloca las dos manos encima del lector a un palmo de distancia";
    }

    void Start()
    {

    }

    void Update()
    {
        listener.refresh();
        firstHandPosition = listener.firstHandPosition;
        secondHandPosition = listener.secondHandPosition;
        firstHandVelocity = listener.firstHandVelocity;
        secondHandVelocity = listener.secondHandVelocity;
        switch (currentGameNumber)
        {
            case 0:
                {
                    clapPanel.SetActive(false);
                    Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                    temp.a = 0f;
                    clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                    temp = clapAlertRawImage.color;
                    temp.a = 0f;
                    clapAlertRawImage.color = temp;
                    clapAlertText.SetActive(false);
                    ball.SetActive(false);
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("introFadeIn"))
                    {
                        alertText.text = "Introduce tu edad en el campo de texto y selecciona tu género.";
                    }
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("introIdle"))
                    {
                        alertText.text = "Introduce tu edad en el campo de texto y selecciona tu género.";
                        int n;
                        bool isNumeric = int.TryParse(ageTextInputfield.text, out n);
                        if(genre != "")
                        {
                            switch (genre)
                            {
                                case "MALE":
                                    {
                                        temp = maleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        maleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        break;
                                    }
                                case "FEMALE":
                                    {
                                        temp = maleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        maleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        break;
                                    }
                                case "UNDEFINED":
                                    {
                                        temp = maleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        maleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0.5f;
                                        femaleGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        undefinedGenreButton.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        break;
                                    }
                            }
                            if (isNumeric)
                            {
                                startIntroButton.interactable = true;
                                if (introPassed)
                                {
                                    currentGameNumber = 1;
                                    age = n;
                                    animator.SetTrigger("introFinished");
                                } else
                                {
                                    introPassed = false;
                                }

                            } else
                            {
                                introPassed = false;
                                startIntroButton.interactable = false;
                            }
                        } else
                        {
                            startIntroButton.interactable = false;
                        }
                    }
                    break;
                }
            case 1:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("introFade"))
                    {
                        clapPanel.SetActive(false);
                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                        temp.a = 0f;
                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                        temp = clapAlertRawImage.color;
                        temp.a = 0f;
                        clapAlertRawImage.color = temp;
                        clapAlertText.SetActive(false);
                    }
                    ball.SetActive(false);
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise1Idle1") || activeGame)
                    {
                        activeGame = true;
                        if (checkHandsAreActive() && listener.firstHandGrabStrength >= grabStrengthMinumum && listener.secondHandGrabStrength >= grabStrengthMinumum)
                        {
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise1Idle1"))
                            {
                                clapPanel.SetActive(true);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 1.0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 1.0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(true);
                                alertRawImage.texture = greenAlertTexture;
                            }
                            fistTextGameObject.SetActive(true);
                            timeExercise1 += Time.deltaTime;
                            timeExercise1 = (timeExercise1 <= 0) ? 0 : timeExercise1;
                            if ((numberOfSecondsCalibrating - timeExercise1) >= 1)
                            {
                                fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Espera " + (numberOfSecondsCalibrating - timeExercise1).ToString("#.##") + " segundos";
                            }
                            else
                            {
                                fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Espera 0" + (numberOfSecondsCalibrating - timeExercise1).ToString(".##") + " segundos";
                            }
                            animator.SetBool("handsActive", true);
                            // Save palm velocity magnitude
                            string firstNumber = (firstHandVelocity.Magnitude >= 1000)? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                            string secondNumber = (secondHandVelocity.Magnitude >= 1000)? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                            string velocityValues = firstNumber + ";" + secondNumber;
                            palmVelocity1 += firstNumber + ";";
                            palmVelocity2 += secondNumber + ";";
                            firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            sbExercise1.Append("\n").Append(velocityValues);

                            // Save palm position
                            palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(palmPositionValues);
                            firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(palmPositionValues);

                            // Save fingers position first hand
                            firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            // Save fingers position second hand
                            firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                            thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                            fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                            sbExercise1.Append(";").Append(fingerPositionValues);

                            // Save grab strength
                            firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                            secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                            string grabStrengthValues = firstNumber + ";" + secondNumber;
                            grabStrength1 += firstNumber + ";";
                            grabStrength2 += secondNumber + ";";
                            sbExercise1.Append(";").Append(grabStrengthValues);

                            numberOfInteractionsFirstHand++;
                            numberOfInteractionsSecondHand++;
                            accumulatedVelocitySecondHand += secondHandVelocity.Magnitude;
                            accumulatedVelocityFirstHand += firstHandVelocity.Magnitude;
                            if (listener.firstHandGrabStrength < minimumGrabStrengthFirstHand)
                            {
                                minimumGrabStrengthFirstHand = listener.firstHandGrabStrength;
                            }
                            if (listener.secondHandGrabStrength < minimumGrabStrengthSecondHand)
                            {
                                minimumGrabStrengthSecondHand = listener.secondHandGrabStrength;
                            }
                            if (listener.firstHandGrabStrength > maximumGrabStrengthFirstHand)
                            {
                                maximumGrabStrengthFirstHand = listener.firstHandGrabStrength;
                            }
                            if (listener.secondHandGrabStrength > maximumGrabStrengthSecondHand)
                            {
                                maximumGrabStrengthSecondHand = listener.secondHandGrabStrength;
                            }

                            //UnityEngine.Debug.Log(firstHandVelocity.Magnitude);
                            //UnityEngine.Debug.Log(secondHandVelocity.Magnitude);
                            if (timeExercise1 > numberOfSecondsCalibrating)
                            {
                                saveFile(sbExercise1.ToString(), currentGameNumber);
                                if (saveDataInDatabaseOption)
                                {
                                    StartCoroutine(SaveDataInDatabase(1));
                                }
                                UnityEngine.Debug.Log("Exercise 1 finished");
                                fistTextGameObject.SetActive(false);
                                activeGame = false;
                                animator.SetTrigger("exercise1Finished");
                                alertText.text = "¡Enhorabuena has completado el primer ejercicio de calibración!\nAhora empiezan los juegos para detectar tu capacidad psicomotriz.";
                                if (launchMatlabGraphAnalysis)
                                {
                                    MatlabFunctionCaller(1, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                    MatlabFunctionCaller(1, typeMatlabGraph.HANDVELOCITY.ToString());
                                    MatlabFunctionCaller(1, typeMatlabGraph.GRABSTRENGTH.ToString());
                                }
                                currentGameNumber = 2;
                            }
                        }
                        else
                        {
                            clapPanel.SetActive(false);
                            Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                            temp.a = 0f;
                            clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                            temp = clapAlertRawImage.color;
                            temp.a = 0f;
                            clapAlertRawImage.color = temp;
                            clapAlertText.SetActive(false);
                            alertRawImage.texture = orangeAlertTexture;
                            timeExercise1 = 0;
                            animator.SetBool("handsActive", false);
                            alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia haciendo un puño";
                        }
                    }
                    else
                    {
                        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("introFade") && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("introIdle"))
                        {
                            clapPanel.SetActive(true);
                            Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                            temp.a = 1.0f;
                            clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                            temp = clapAlertRawImage.color;
                            temp.a = 1.0f;
                            clapAlertRawImage.color = temp;
                            clapAlertText.SetActive(true);
                        }
                        alertRawImage.texture = greenAlertTexture;
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise1Idle"))
                        {
                            animator.SetTrigger("intro");
                            alertText.text = "A continuación se realizarán unas tareas siguiendo los pasos mostrados.";
                        }
                        else
                        {
                            if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                            {
                                animator.SetTrigger("nextIntroStep");
                                clapSound.Play();
                            }
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise1PopUpAlertIntro1_2"))
                            {
                                alertText.text = "En el primer ejercicio el objetivo será cerrar las manos haciendo un puño durante " + numberOfSecondsCalibrating + " segundos en paralelo con la palma mirando al suelo.";
                            }
                        }
                    }
                    break;
                }
            case 2:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise1EndPopUpAlertIdle") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2PopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    else
                    {
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2PopUpAlert"))
                        {
                            alertText.text = "En este ejercicio el objetivo será con la palma de las manos mirando al suelo un poco curvadas intentar crear una burbuja elevándolas a la vez haciendo una trayectoria en V.";

                        }
                        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("fadeOutExercise2"))
                        {
                            ball.SetActive(true);
                            ball.GetComponent<MeshRenderer>().material = bubbleMaterial;
                        }
                        else
                        {
                            System.Random r = new System.Random();
                            int rInt = r.Next(0, 100);
                            if (rInt > 80)
                            {
                                ball.transform.eulerAngles += new Vector3(0.0f, 0.3f, 0.3f);
                            }
                            else
                            {
                                ball.transform.eulerAngles += new Vector3(0.3f, 0.0f, 0.0f);
                            }
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2Idle1") || activeGame)
                            {
                                if (checkHandsAreActive())
                                {
                                    if (activeGame || (((minimumDistanceExercise2) > ((Math.Sqrt(Math.Pow(secondHandPosition.x - ball.transform.position.x, 2) +
                                        Math.Pow(secondHandPosition.y - ball.transform.position.y, 2) + Math.Pow(secondHandPosition.z - ball.transform.position.z, 2))) +
                                        Math.Sqrt(Math.Pow(firstHandPosition.x - ball.transform.position.x, 2) + Math.Pow(firstHandPosition.y - ball.transform.position.y, 2) +
                                            Math.Pow(firstHandPosition.z - ball.transform.position.z, 2)))) && (50 > Math.Abs(firstHandPosition.y - secondHandPosition.y))))
                                    {
                                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2Idle1"))
                                        {
                                            clapPanel.SetActive(true);
                                            Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                            temp.a = 1.0f;
                                            clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                            temp = clapAlertRawImage.color;
                                            temp.a = 1.0f;
                                            clapAlertRawImage.color = temp;
                                            clapAlertText.SetActive(true);
                                            alertRawImage.texture = greenAlertTexture;
                                        }
                                        activeGame = true;
                                        animator.SetBool("handsActive", true);
                                        xHandToBallDistanceFirstHandPartialValue = Math.Abs(ball.transform.position.x - firstHandPosition.x);
                                        yHandToBallDistanceFirstHandPartialValue = Math.Abs(ball.transform.position.y - firstHandPosition.y);
                                        xHandToBallDistanceSecondHandPartialValue = Math.Abs(ball.transform.position.x - secondHandPosition.x);
                                        yHandToBallDistanceSecondHandPartialValue = Math.Abs(ball.transform.position.y - secondHandPosition.y);
                                        if (xHandToBallDistanceFirstHandPartialValue > xHandToBallDistanceFirstHand && yHandToBallDistanceFirstHandPartialValue > yHandToBallDistanceFirstHand &&
                                            xHandToBallDistanceSecondHandPartialValue > xHandToBallDistanceSecondHand && yHandToBallDistanceSecondHandPartialValue > yHandToBallDistanceSecondHand)
                                        {
                                            xHandToBallDistanceFirstHand = xHandToBallDistanceFirstHandPartialValue;
                                            yHandToBallDistanceFirstHand = yHandToBallDistanceFirstHandPartialValue;
                                            xHandToBallDistanceSecondHand = xHandToBallDistanceSecondHandPartialValue;
                                            yHandToBallDistanceSecondHand = yHandToBallDistanceSecondHandPartialValue;
                                            scaleChange = new Vector3(Math.Abs(yHandToBallDistanceFirstHand + yHandToBallDistanceSecondHand) / 300000, Math.Abs(yHandToBallDistanceFirstHand + yHandToBallDistanceSecondHand) / 300000, 0f);
                                            scaleChange.x = (scaleChange.x > 0.01f) ? 0.015f : scaleChange.x;
                                            scaleChange.y = (scaleChange.y > 0.01f) ? 0.015f : scaleChange.y;
                                            scaleChange.z = (scaleChange.y + scaleChange.x) / 2;
                                            ball.transform.localScale += scaleChange;
                                            NumberFormatInfo nfi = new NumberFormatInfo();
                                            nfi.NumberDecimalSeparator = ".";
                                            // Save palm velocity magnitude
                                            string firstNumber = (firstHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                            string secondNumber = (secondHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                            string velocityValues = firstNumber + ";" + secondNumber;
                                            palmVelocity1 += firstNumber + ";";
                                            palmVelocity2 += secondNumber + ";";
                                            firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            sbExercise2.Append("\n").Append(velocityValues);

                                            // Save palm position
                                            palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(palmPositionValues);
                                            firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(palmPositionValues);

                                            // Save fingers position first hand
                                            firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            // Save fingers position second hand
                                            firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                            thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                            fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                            fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                            sbExercise2.Append(";").Append(fingerPositionValues);

                                            // Save grab strength
                                            firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                            secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                            string grabStrengthValues = firstNumber + ";" + secondNumber;
                                            grabStrength1 += firstNumber + ";";
                                            grabStrength2 += secondNumber + ";";
                                            sbExercise2.Append(";").Append(grabStrengthValues);
                                            //UnityEngine.Debug.Log(firstHandVelocity.Magnitude);
                                            //UnityEngine.Debug.Log(secondHandVelocity.Magnitude);
                                            if (ball.transform.localScale.x > 0.28 && ball.transform.localScale.y > 0.28)
                                            {
                                                if (saveDataInDatabaseOption)
                                                {
                                                    StartCoroutine(SaveDataInDatabase(2));
                                                }
                                                saveFile(sbExercise2.ToString(),currentGameNumber);
                                                UnityEngine.Debug.Log("Exercise 2 finished");
                                                activeGame = false;
                                                animator.SetTrigger("exercise2Finished");
                                                alertText.text = "¡Enhorabuena has completado el segundo ejercicio!";
                                                if (launchMatlabGraphAnalysis)
                                                {
                                                    MatlabFunctionCaller(2, typeMatlabGraph.HANDVELOCITY.ToString());
                                                    MatlabFunctionCaller(2, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                                }
                                                currentGameNumber = 3;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        clapPanel.SetActive(false);
                                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0f;
                                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = clapAlertRawImage.color;
                                        temp.a = 0f;
                                        clapAlertRawImage.color = temp;
                                        clapAlertText.SetActive(false);
                                        alertRawImage.texture = orangeAlertTexture;
                                        animator.SetBool("handsActive", false);
                                        alertText.text = "Coloca las manos más cerca del lector a la misma altura";
                                    }
                                }
                                else
                                {
                                    clapPanel.SetActive(false);
                                    Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                    temp.a = 0f;
                                    clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                    temp = clapAlertRawImage.color;
                                    temp.a = 0f;
                                    clapAlertRawImage.color = temp;
                                    clapAlertText.SetActive(false);
                                    alertRawImage.texture = orangeAlertTexture;
                                    animator.SetBool("handsActive", false);
                                    alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                                }
                            }
                            else if (checkHandsAreActive() && this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2AlertIdle1") && (((minimumDistanceExercise2) > ((Math.Sqrt(Math.Pow(secondHandPosition.x - ball.transform.position.x, 2) +
                                        Math.Pow(secondHandPosition.y - ball.transform.position.y, 2) + Math.Pow(secondHandPosition.z - ball.transform.position.z, 2))) +
                                        Math.Sqrt(Math.Pow(firstHandPosition.x - ball.transform.position.x, 2) + Math.Pow(firstHandPosition.y - ball.transform.position.y, 2) +
                                            Math.Pow(firstHandPosition.z - ball.transform.position.z, 2)))) && (50 > Math.Abs(firstHandPosition.y - secondHandPosition.y))))
                                
                            {
                                animator.SetBool("handsActive", true);
                            }
                        }
                    }
                    break;
                }
            case 3:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise2EndPopUpAlertIdle") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise3PopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    else
                    {
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise3PopUpAlert"))
                        {
                            alertText.text = "En este ejercicio el objetivo será limpiar la parte superior de la bola con ambas palmas de la mano.";
                            mediumVelocityFirstHand = accumulatedVelocityFirstHand / numberOfInteractionsFirstHand;
                            mediumVelocitySecondHand = accumulatedVelocitySecondHand / numberOfInteractionsSecondHand;

                        }
                        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("fadeOutExercise3"))
                        {
                            ball.transform.eulerAngles = new Vector3(18.0f, 0.0f, 0.0f);
                            ball.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                            Color lastColor = dirtBallMesh.materials[0].color;
                            dirtBallMesh.materials[0].color = new Color(lastColor.r, lastColor.g, lastColor.b, 0);
                            ball.SetActive(true);
                            dirtyBall.SetActive(true);
                            cleaningText.SetActive(true);
                            ball.GetComponent<MeshRenderer>().material = footballBallMaterial;
                            sliderBarGameObject.SetActive(true);
                        }
                        else
                        {
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise3Idle1") || activeGame)
                            {
                                activeGame = true;
                                sliderBarGameObject.SetActive(true);
                                cleaningText.SetActive(true);
                                if (checkHandsAreActive())
                                {
                                    animator.SetBool("handsActive", true);
                                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise3Idle1"))
                                    {
                                        clapPanel.SetActive(true);
                                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = clapAlertRawImage.color;
                                        temp.a = 1.0f;
                                        clapAlertRawImage.color = temp;
                                        clapAlertText.SetActive(true);
                                        alertRawImage.texture = greenAlertTexture;
                                    }
                                    NumberFormatInfo nfi = new NumberFormatInfo();
                                    nfi.NumberDecimalSeparator = ".";
                                    // Save palm velocity magnitude
                                    string firstNumber = (firstHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                    string secondNumber = (secondHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                    string velocityValues = firstNumber + ";" + secondNumber;
                                    palmVelocity1 += firstNumber + ";";
                                    palmVelocity2 += secondNumber + ";";
                                    firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    sbExercise3.Append("\n").Append(velocityValues);

                                    // Save palm position
                                    palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(palmPositionValues);
                                    firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(palmPositionValues);

                                    // Save fingers position first hand
                                    firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    // Save fingers position second hand
                                    firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                    thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                    fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                    fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                    sbExercise3.Append(";").Append(fingerPositionValues);

                                    // Save grab strength
                                    firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                    secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                    string grabStrengthValues = firstNumber + ";" + secondNumber;
                                    grabStrength1 += firstNumber + ";";
                                    grabStrength2 += secondNumber + ";";
                                    sbExercise3.Append(";").Append(grabStrengthValues);
                                    //UnityEngine.Debug.Log(firstHandVelocity.Magnitude);
                                    //UnityEngine.Debug.Log(secondHandVelocity.Magnitude);
                                    //gameObject.GetComponent<Renderer> ().material.color = mycolor;
                                    if (dirtBallMesh != null)
                                    {
                                        if ((exercise3Height > (Math.Pow(firstHandPosition.x - ball.transform.position.x, 2) + Math.Pow(firstHandPosition.y - ball.transform.position.y, 2) +
                                            Math.Pow(firstHandPosition.z - ball.transform.position.z, 2)) && firstHandVelocity.Magnitude > (handVelocityExercise3 + mediumVelocityFirstHand)) ||
                                            (exercise3Height > (Math.Sqrt(Math.Pow(secondHandPosition.x - ball.transform.position.x, 2) + Math.Pow(secondHandPosition.y - ball.transform.position.y, 2) +
                                            Math.Pow(secondHandPosition.z - ball.transform.position.z, 2))) && secondHandVelocity.Magnitude > (handVelocityExercise3 + mediumVelocitySecondHand)))
                                        {
                                            Color lastColor = dirtBallMesh.materials[0].color;
                                            System.Random r = new System.Random();
                                            int rInt = r.Next(0, 100);
                                            if (rInt > 90)
                                            {
                                                dirtBallMesh.materials[0].color = new Color(lastColor.r, lastColor.g, lastColor.b, lastColor.a + 1);
                                                sliderBarGameObject.GetComponent<UnityEngine.UI.Slider>().value += 3 / 255f;
                                            }
                                        }
                                    }
                                    if (sliderBarGameObject.GetComponent<UnityEngine.UI.Slider>().value >= 1)
                                    {
                                        saveFile(sbExercise3.ToString(), currentGameNumber);
                                        if (launchMatlabGraphAnalysis)
                                        {
                                            MatlabFunctionCaller(3, typeMatlabGraph.HANDVELOCITY.ToString());
                                            MatlabFunctionCaller(3, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                            MatlabFunctionCaller(3, typeMatlabGraph.FINGERSPOSITION.ToString());
                                        }
                                        if (saveDataInDatabaseOption)
                                        {
                                            StartCoroutine(SaveDataInDatabase(3));
                                        }
                                        currentGameNumber = 4;
                                        dirtyBall.SetActive(false);
                                        ball.SetActive(false);
                                        sliderBarGameObject.SetActive(false);
                                        cleaningText.SetActive(false);
                                        UnityEngine.Debug.Log("Exercise 3 finished");
                                        alertText.text = "¡Enhorabuena has completado el tercer ejercicio!";
                                        animator.SetTrigger("exercise3Finished");
                                        activeGame = false;
                                    }
                                }
                                else
                                {
                                    if (checkHandsAreActive())
                                    {
                                        animator.SetBool("handsActive", true);
                                    } else if(sliderBarGameObject.GetComponent<UnityEngine.UI.Slider>().value < 1)
                                    {
                                        clapPanel.SetActive(false);
                                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 0f;
                                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = clapAlertRawImage.color;
                                        temp.a = 0f;
                                        clapAlertRawImage.color = temp;
                                        clapAlertText.SetActive(false);
                                        alertRawImage.texture = orangeAlertTexture;
                                        animator.SetBool("handsActive", false);
                                        alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                                    }
                                }
                            } else if (checkHandsAreActive())
                            {
                                animator.SetBool("handsActive", true);
                            }
                        }
                    }
                    break;
                }
            case 4:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise3EndPopUpAlertIdle") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4PopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    else
                    {
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4PopUpAlert"))
                        {
                            alertText.text = "En este ejercicio el objetivo será doblar los dedos de ambas manos según se indique hacia el interior de la palma lo más rápido posible.";

                        }
                        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("fadeOutExercise4"))
                        {
                            fingersIndicatorTextGameObject.SetActive(true);
                            timeExercise4TextGameObject.SetActive(false);
                            ball.SetActive(false);
                        }
                        else
                        {
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4Idle1") || activeGame)
                            {
                                activeGame = true;
                                if (checkHandsAreActive())
                                {
                                    animator.SetBool("handsActive", true);
                                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4Idle1"))
                                    {
                                        timeExercise4 += Time.deltaTime;
                                        timeExercise4PerFinger += Time.deltaTime;
                                        timeExercise4TextGameObject.GetComponent<UnityEngine.UI.Text>().text = timeExercise4.ToString("#.##");
                                        clapPanel.SetActive(true);
                                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = clapAlertRawImage.color;
                                        temp.a = 1.0f;
                                        clapAlertRawImage.color = temp;
                                        clapAlertText.SetActive(true);
                                        alertRawImage.texture = greenAlertTexture;
                                        if (exercise4StepPassed)
                                        {
                                            exercise4StepPassed = false;
                                            System.Random r = new System.Random();
                                            int rInt = r.Next(0, 5);
                                            currentFinger = rInt;
                                            fingersIndicatorTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Dobla el dedo " + fingerNouns[rInt];
                                        }
                                        else
                                        {
                                            if (timeExercise4PerFinger > limitTimePerFingerExercise4)
                                            {
                                                timeExercise4PerFinger = 0;
                                                exercise4StepPassed = true;
                                                exercise4NumberOfStepsPassed++;
                                            }
                                            /*
                                            Vector firstHand = Vector.Zero;
                                            Vector secondHand = Vector.Zero;
                                            if ((Math.Sqrt(Math.Pow(listener.thumbTipFirstHandPosition.x - listener.firstHandPosition.x, 2) +
                                                            Math.Pow(listener.thumbTipFirstHandPosition.y - listener.firstHandPosition.y, 2) +
                                                            Math.Pow(listener.thumbTipFirstHandPosition.z - listener.firstHandPosition.z, 2))) < 
                                                            (Math.Sqrt(Math.Pow(listener.thumbTipFirstHandPosition.x - listener.secondHandPosition.x, 2) +
                                                            Math.Pow(listener.thumbTipFirstHandPosition.y - listener.secondHandPosition.y, 2) +
                                                            Math.Pow(listener.thumbTipFirstHandPosition.z - listener.secondHandPosition.z, 2))))
                                            {
                                                firstHand = firstHandPosition;
                                                secondHand = secondHandPosition;
                                            } else
                                            {
                                                firstHand = secondHandPosition;
                                                secondHand = firstHandPosition;
                                            }
                                            UnityEngine.Debug.Log((Math.Sqrt(Math.Pow(listener.thumbTipFirstHandPosition.x-firstHand.x,2) + 
                                            Math.Pow(listener.thumbTipFirstHandPosition.y - firstHand.y, 2) +
                                            Math.Pow(listener.thumbTipFirstHandPosition.z - firstHand.z, 2))) + "-" + (Math.Sqrt(Math.Pow(listener.thumbTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.thumbTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.thumbTipSecondHandPosition.z - secondHand.z, 2))));
                                            switch (currentFinger)
                                            {
                                                case 0:
                                                    {
                                                        if ((Math.Sqrt(Math.Pow(listener.thumbTipFirstHandPosition.x-firstHand.x,2) + 
                                                            Math.Pow(listener.thumbTipFirstHandPosition.y - firstHand.y, 2) +
                                                            Math.Pow(listener.thumbTipFirstHandPosition.z - firstHand.z, 2)) < distanceFingerToHand) &&
                                                                (Math.Sqrt(Math.Pow(listener.thumbTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.thumbTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.thumbTipSecondHandPosition.z - secondHand.z, 2)) < distanceFingerToHand))
                                                        {
                                                            exercise4StepPassed = true;
                                                        }
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        if ((Math.Sqrt(Math.Pow(listener.indexTipFirstHandPosition.x - firstHand.x, 2) +
                                                            Math.Pow(listener.indexTipFirstHandPosition.y - firstHand.y, 2) +
                                                            Math.Pow(listener.indexTipFirstHandPosition.z - firstHand.z, 2)) < distanceFingerToHand) &&
                                                                (Math.Sqrt(Math.Pow(listener.indexTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.indexTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.indexTipSecondHandPosition.z - secondHand.z, 2)) < distanceFingerToHand))
                                                        {
                                                            exercise4StepPassed = true;
                                                        }
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        if ((Math.Sqrt(Math.Pow(listener.ringTipFirstHandPosition.x - firstHand.x, 2) +
                                                            Math.Pow(listener.ringTipFirstHandPosition.y - firstHand.y, 2) +
                                                            Math.Pow(listener.ringTipFirstHandPosition.z - firstHand.z, 2)) < distanceFingerToHand) &&
                                                                (Math.Sqrt(Math.Pow(listener.ringTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.ringTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.ringTipSecondHandPosition.z - secondHand.z, 2)) < distanceFingerToHand))
                                                        {
                                                            exercise4StepPassed = true;
                                                        }
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        if ((Math.Sqrt(Math.Pow(listener.middleTipFirstHandPosition.x - firstHand.x, 2) +
                                                            Math.Pow(listener.middleTipFirstHandPosition.y - firstHand.y, 2) +
                                                            Math.Pow(listener.middleTipFirstHandPosition.z - firstHand.z, 2)) < distanceFingerToHand) &&
                                                                (Math.Sqrt(Math.Pow(listener.middleTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.middleTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.middleTipSecondHandPosition.z - secondHand.z, 2)) < distanceFingerToHand))
                                                        {
                                                            exercise4StepPassed = true;
                                                        }
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        if ((Math.Sqrt(Math.Pow(listener.pinkyTipFirstHandPosition.x - firstHand.x, 2) +
                                                            Math.Pow(listener.pinkyTipFirstHandPosition.y - firstHand.y, 2) +
                                                            Math.Pow(listener.pinkyTipFirstHandPosition.z - firstHand.z, 2)) < distanceFingerToHand) &&
                                                                (Math.Sqrt(Math.Pow(listener.pinkyTipSecondHandPosition.x - secondHand.x, 2) +
                                                            Math.Pow(listener.pinkyTipSecondHandPosition.y - secondHand.y, 2) +
                                                            Math.Pow(listener.pinkyTipSecondHandPosition.z - secondHand.z, 2)) < distanceFingerToHand))
                                                        {
                                                            exercise4StepPassed = true;
                                                        }
                                                        break;
                                                    }
                                            */
                                        }
                                        NumberFormatInfo nfi = new NumberFormatInfo();
                                        nfi.NumberDecimalSeparator = ".";
                                        // Save palm velocity magnitude
                                        string firstNumber = (firstHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string secondNumber = (secondHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string velocityValues = firstNumber + ";" + secondNumber;
                                        palmVelocity1 += firstNumber + ";";
                                        palmVelocity2 += secondNumber + ";";
                                        firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        sbExercise4.Append("\n").Append(velocityValues);

                                        // Save palm position
                                        palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(palmPositionValues);
                                        firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(palmPositionValues);

                                        // Save fingers position first hand
                                        firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        // Save fingers position second hand
                                        firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise4.Append(";").Append(fingerPositionValues);

                                        // Save grab strength
                                        firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        string grabStrengthValues = firstNumber + ";" + secondNumber;
                                        grabStrength1 += firstNumber + ";";
                                        grabStrength2 += secondNumber + ";";
                                        sbExercise4.Append(";").Append(grabStrengthValues);

                                        //UnityEngine.Debug.Log(firstHandVelocity.Magnitude);
                                        //UnityEngine.Debug.Log(secondHandVelocity.Magnitude);
                                        //gameObject.GetComponent<Renderer> ().material.color = mycolor;                                    
                                        if (exercise4NumberOfStepsPassed == 5)
                                        {
                                            saveFile(sbExercise4.ToString(), currentGameNumber);
                                            UnityEngine.Debug.Log("Exercise 4 finished");
                                            activeGame = false;
                                            fingersIndicatorTextGameObject.SetActive(false);
                                            timeExercise4TextGameObject.SetActive(false);
                                            animator.SetTrigger("exercise4Finished");
                                            alertText.text = "¡Enhorabuena has completado el cuarto ejercicio!";
                                            if (launchMatlabGraphAnalysis)
                                            {
                                                MatlabFunctionCaller(4, typeMatlabGraph.HANDVELOCITY.ToString());
                                                MatlabFunctionCaller(4, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                            }
                                            if (saveDataInDatabaseOption)
                                            {
                                                StartCoroutine(SaveDataInDatabase(4));
                                            }
                                            currentGameNumber = 5;
                                        }
                                    }
                                } else
                                {
                                    clapPanel.SetActive(false);
                                    Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                    temp.a = 0f;
                                    clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                    temp = clapAlertRawImage.color;
                                    temp.a = 0f;
                                    clapAlertRawImage.color = temp;
                                    clapAlertText.SetActive(false);
                                    alertRawImage.texture = orangeAlertTexture;
                                    animator.SetBool("handsActive", false);
                                    alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                                }
                            }
                            else if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4Idle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4PopUpAlert1")
                                || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4AlertIdle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4FadeAlert1"))
                            {
                                clapPanel.SetActive(false);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(false);
                                alertRawImage.texture = orangeAlertTexture;
                                animator.SetBool("handsActive", false);
                                alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                            }
                            else
                            {
                                clapPanel.SetActive(true);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 1.0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 1.0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(true);
                                alertRawImage.texture = greenAlertTexture;
                            }
                        }
                    }
                    break;
                }
            case 5:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise4EndPopUpAlertIdle") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5PopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    else
                    {
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5PopUpAlert"))
                        {
                            alertText.text = "En este ejercicio el objetivo será abrir y cerrar las manos formando un puño lo más rápido posible.";

                        }
                        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("fadeOutExercise5"))
                        {
                            fistTextGameObject.SetActive(false);
                        }
                        else
                        {
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5Idle1") || activeGame)
                            {
                                fistTextGameObject.SetActive(true);
                                activeGame = true;
                                if (checkHandsAreActive())
                                {
                                    animator.SetBool("handsActive", true);
                                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5Idle1"))
                                    {
                                        timeExercise5 -= Time.deltaTime;
                                        timeExercise5 = (timeExercise5 <= 0) ? 0 : timeExercise5;
                                        if (!fistFirstHandActive && (maximumGrabStrengthFirstHand <= listener.firstHandGrabStrength))
                                        {
                                            fistFirstHandActive = true;
                                            fistNumber++; 
                                        } else if (fistFirstHandActive && ((maximumGrabStrengthFirstHand + minimumGrabStrengthFirstHand)/2 > listener.firstHandGrabStrength))
                                        {
                                            fistFirstHandActive = false;
                                        }

                                        if (!fistSecondHandActive && (maximumGrabStrengthSecondHand <= listener.secondHandGrabStrength))
                                        {
                                            fistSecondHandActive = true;
                                            fistNumber++;
                                        }
                                        else if (fistSecondHandActive && ((maximumGrabStrengthSecondHand + minimumGrabStrengthSecondHand)/ 2 > listener.secondHandGrabStrength))
                                        {
                                            fistSecondHandActive = false;
                                        }

                                        if (timeExercise5 >= 1)
                                        {
                                            fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Quedan " + timeExercise5.ToString("#.##") + " segundos\nNúmero de puños: " + fistNumber;
                                        }
                                        else
                                        {
                                            fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Quedan 0" + timeExercise5.ToString(".##") + " segundos\nNúmero de puños: " + fistNumber;
                                        }
                                        NumberFormatInfo nfi = new NumberFormatInfo();
                                        nfi.NumberDecimalSeparator = ".";
                                        // Save palm velocity magnitude
                                        string firstNumber = (firstHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string secondNumber = (secondHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string velocityValues = firstNumber + ";" + secondNumber;
                                        palmVelocity1 += firstNumber + ";";
                                        palmVelocity2 += secondNumber + ";";
                                        firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        sbExercise5.Append("\n").Append(velocityValues);

                                        // Save palm position
                                        palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(palmPositionValues);
                                        firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(palmPositionValues);

                                        // Save fingers position first hand
                                        firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        // Save fingers position second hand
                                        firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise5.Append(";").Append(fingerPositionValues);

                                        // Save grab strength
                                        firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        string grabStrengthValues = firstNumber + ";" + secondNumber;
                                        grabStrength1 += firstNumber + ";";
                                        grabStrength2 += secondNumber + ";";
                                        sbExercise5.Append(";").Append(grabStrengthValues);
                                        //UnityEngine.Debug.Log(firstHandVelocity.Magnitude);
                                        //UnityEngine.Debug.Log(secondHandVelocity.Magnitude);
                                        if (timeExercise5 == 0)
                                        {
                                            if (saveDataInDatabaseOption)
                                            {
                                                StartCoroutine(SaveDataInDatabase(5));
                                            }
                                            saveFile(sbExercise5.ToString(), currentGameNumber);
                                            UnityEngine.Debug.Log("Exercise 5 finished");
                                            activeGame = false;
                                            animator.SetTrigger("exercise5Finished");
                                            clapPanel.SetActive(true);
                                            Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                            temp.a = 1.0f;
                                            clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                            temp = clapAlertRawImage.color;
                                            temp.a = 1.0f;
                                            clapAlertRawImage.color = temp;
                                            clapAlertText.SetActive(true);
                                            alertRawImage.texture = greenAlertTexture;
                                            fistTextGameObject.SetActive(false);
                                            alertText.text = "¡Enhorabuena has completado el quinto ejercicio!";
                                            if (launchMatlabGraphAnalysis)
                                            {
                                                MatlabFunctionCaller(5, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                                MatlabFunctionCaller(5, typeMatlabGraph.HANDVELOCITY.ToString());
                                            }
                                            currentGameNumber = 6;
                                        }
                                    }
                                }
                                else
                                {
                                    alertRawImage.texture = orangeAlertTexture;
                                    clapPanel.SetActive(false);
                                    Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                    temp.a = 0f;
                                    clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                    temp = clapAlertRawImage.color;
                                    temp.a = 0f;
                                    clapAlertRawImage.color = temp;
                                    clapAlertText.SetActive(false);
                                    animator.SetBool("handsActive", false);
                                    alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                                }
                            }
                            else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5Idle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5PopUpAlert1")
                                || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5AlertIdle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5FadeAlert1"))
                            {
                                alertRawImage.texture = orangeAlertTexture;
                                clapPanel.SetActive(false);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(false);
                                animator.SetBool("handsActive", false);
                                alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                            }
                            else
                            {
                                clapPanel.SetActive(true);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 1.0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 1.0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(true);
                                alertRawImage.texture = greenAlertTexture;
                            }
                        }
                    }
                    break;
                }
            case 6:
                {
                    
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise5EndPopUpAlertIdle") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6PopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    else
                    {
                        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6PopUpAlert"))
                        {
                            alertText.text = "En este ejercicio el objetivo será subir y bajar las manos con la palma hacia abajo de forma inversa cada mano de tal forma que cuando una suba la otra baje durante 10 segundos.";

                        }
                        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("fadeOutExercise6"))
                        {
                            fistTextGameObject.SetActive(false);
                        }
                        else
                        {
                            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6Idle1") || activeGame)
                            {
                                fistTextGameObject.SetActive(true);
                                activeGame = true;
                                if (checkHandsAreActive())
                                {
                                    animator.SetBool("handsActive", true);
                                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6Idle1"))
                                    {
                                        Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                        temp.a = 1.0f;
                                        clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                        temp = clapAlertRawImage.color;
                                        temp.a = 1.0f;
                                        clapAlertRawImage.color = temp;
                                        clapAlertText.SetActive(true);
                                        timeExercise6 -= Time.deltaTime;
                                        timeExercise6 = (timeExercise6 <= 0) ? 0 : timeExercise6;
                                        if (timeExercise6 >= 1)
                                        {
                                            fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Sube y baja las manos durante " + timeExercise6.ToString("#.##") + " segundos";
                                        }
                                        else
                                        {
                                            fistTextGameObject.GetComponent<UnityEngine.UI.Text>().text = "Sube y baja las manos durante " + "0"+ timeExercise6.ToString(".##") + " segundos";
                                        }
                                        NumberFormatInfo nfi = new NumberFormatInfo();
                                        nfi.NumberDecimalSeparator = ".";
                                        // Save palm velocity magnitude
                                        string firstNumber = (firstHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(firstHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string secondNumber = (secondHandVelocity.Magnitude >= 1000) ? ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(secondHandVelocity.Magnitude.ToString(), ",", ".").Replace(",", "");
                                        string velocityValues = firstNumber + ";" + secondNumber;
                                        palmVelocity1 += firstNumber + ";";
                                        palmVelocity2 += secondNumber + ";";
                                        firstNumber = (listener.firstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.firstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        string thirdNumber = (listener.firstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        sbExercise6.Append("\n").Append(velocityValues);

                                        // Save palm position
                                        palmPosition1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(palmPositionValues);
                                        firstNumber = (listener.secondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.secondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        palmPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        palmPosition2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(palmPositionValues);

                                        // Save fingers position first hand
                                        firstNumber = (listener.thumbTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        string fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipFirstHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipFirstHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipFirstHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipFirstHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand1 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        // Save fingers position second hand
                                        firstNumber = (listener.thumbTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.thumbTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.thumbTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.thumbTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition1_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.indexTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.indexTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.indexTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.indexTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition2_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.ringTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.ringTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.ringTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.ringTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition3_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.middleTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.middleTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.middleTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.middleTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition4_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        firstNumber = (listener.pinkyTipSecondHandPosition.x >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.x.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.pinkyTipSecondHandPosition.y >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.y.ToString(), ",", ".").Replace(",", "");
                                        thirdNumber = (listener.pinkyTipSecondHandPosition.z >= 1000) ? ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.pinkyTipSecondHandPosition.z.ToString(), ",", ".").Replace(",", "");
                                        fingerPositionValues = firstNumber + ";" + secondNumber + ";" + thirdNumber;
                                        fingersPosition5_Hand2 += firstNumber + ";" + secondNumber + ";" + thirdNumber + ";";
                                        sbExercise6.Append(";").Append(fingerPositionValues);

                                        // Save grab strength
                                        firstNumber = (listener.firstHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.firstHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        secondNumber = (listener.secondHandGrabStrength >= 1000) ? ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "") : ReplaceLastOccurrence(listener.secondHandGrabStrength.ToString(), ",", ".").Replace(",", "");
                                        string grabStrengthValues = firstNumber + ";" + secondNumber;
                                        grabStrength1 += firstNumber + ";";
                                        grabStrength2 += secondNumber + ";";
                                        sbExercise6.Append(";").Append(grabStrengthValues);
                                        if (timeExercise6 == 0)
                                        {
                                            if (saveDataInDatabaseOption)
                                            {
                                                StartCoroutine(SaveDataInDatabase(6));
                                            }
                                            saveFile(sbExercise6.ToString(), currentGameNumber);
                                            UnityEngine.Debug.Log("Exercise 6 finished");
                                            activeGame = false;
                                            animator.SetTrigger("exercise6Finished");
                                            clapPanel.SetActive(true);
                                            temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                            temp.a = 1.0f;
                                            clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                            temp = clapAlertRawImage.color;
                                            temp.a = 1.0f;
                                            clapAlertRawImage.color = temp;
                                            clapAlertText.SetActive(true);
                                            alertRawImage.texture = greenAlertTexture;
                                            fistTextGameObject.SetActive(false);
                                            alertText.text = "¡Enhorabuena has completado el sexto ejercicio!";
                                            if (launchMatlabGraphAnalysis)
                                            {
                                                MatlabFunctionCaller(6, typeMatlabGraph.HANDPALMPOSITION.ToString());
                                                MatlabFunctionCaller(6, typeMatlabGraph.GRABSTRENGTH.ToString());
                                            }
                                            currentGameNumber = 7;
                                        }                                        
                                    }
                                }
                                else
                                {
                                    alertRawImage.texture = orangeAlertTexture;
                                    clapPanel.SetActive(false);
                                    Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                    temp.a = 0f;
                                    clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                    temp = clapAlertRawImage.color;
                                    temp.a = 0f;
                                    clapAlertRawImage.color = temp;
                                    clapAlertText.SetActive(false);
                                    animator.SetBool("handsActive", false);
                                    alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                                }
                            }
                            else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6Idle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6PopUpAlert1")
                                || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6AlertIdle1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6FadeAlert1"))
                            {
                                alertRawImage.texture = orangeAlertTexture;

                                clapPanel.SetActive(false);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(false);
                                animator.SetBool("handsActive", false);
                                alertText.text = "Coloca las dos manos encima del lector a dos palmos de distancia";
                            }
                            else
                            {
                                clapPanel.SetActive(true);
                                Color temp = clapPanel.GetComponent<UnityEngine.UI.Image>().color;
                                temp.a = 1.0f;
                                clapPanel.GetComponent<UnityEngine.UI.Image>().color = temp;
                                temp = clapAlertRawImage.color;
                                temp.a = 1.0f;
                                clapAlertRawImage.color = temp;
                                clapAlertText.SetActive(true);
                                alertRawImage.texture = greenAlertTexture;
                            }
                        }
                    }
                    break;
                }
            case 7:
                {
                    if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("exercise6EndPopUpAlertIdle"))
                    {
                        if ((Math.Abs(firstHandPosition.y - secondHandPosition.y)) < 5 && (Math.Abs(firstHandPosition.x - secondHandPosition.x)) < clapSpeed && listener.hands == 2 &&
                                (listener.firstHandVelocity.Magnitude + listener.secondHandVelocity.Magnitude) > clapSpeed)
                        {
                            animator.SetTrigger("nextIntroStep");
                            clapSound.Play();
                        }
                    }
                    break;
                }
        }

    }

    private bool checkHandsAreActive()
    {
        return listener.hands == 2;
    }

    private void saveFile(string contentEntry, int currentGame)
    {
        /*
        string path = Application.dataPath +"\\" + csvName + currentGame + ".csv";
        File.WriteAllText(path, contentEntry.ToString());
        UnityEngine.Debug.Log("CSV file written to " + path);
        */
        
        string path = Application.dataPath +"\\" + csvName + currentGame + ".csv";
        File.WriteAllText(path, contentEntry, Encoding.UTF8);
        //Uncomment to have appended documents 
        /*
        if (!File.Exists(path))
        {
            File.WriteAllText(path, contentEntry, Encoding.UTF8);
        } else {
            File.AppendAllText(path, contentEntry, Encoding.UTF8);
        }
        */
    }

    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }

    public void PassIntro()
    {
        introPassed = true;
    }

    public void SelectGenre(string genre)
    {
        this.genre = genre;
    }

    private void MatlabFunctionCaller(int number, string type)
    {
        System.Diagnostics.Process.Start(Application.dataPath + "\\run.bat", "\""+ number + "\" \"" + type + "\" \"" + Application.dataPath + "\"");
    }

    IEnumerator SaveDataInDatabase(int exerciseNumber)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID",id);
        form.AddField("AGE",age);
        form.AddField("GENRE",genre);
        form.AddField("GRABSTRENGTH1",grabStrength1);
        form.AddField("GRABSTRENGTH2", grabStrength2);
        form.AddField("FINGERSPOSITION1_HAND1",fingersPosition1_Hand1);
        form.AddField("FINGERSPOSITION2_HAND1", fingersPosition2_Hand1);
        form.AddField("FINGERSPOSITION3_HAND1", fingersPosition3_Hand1);
        form.AddField("FINGERSPOSITION4_HAND1", fingersPosition4_Hand1);
        form.AddField("FINGERSPOSITION5_HAND1", fingersPosition5_Hand1);
        form.AddField("FINGERSPOSITION1_HAND2", fingersPosition1_Hand2);
        form.AddField("FINGERSPOSITION2_HAND2", fingersPosition2_Hand2);
        form.AddField("FINGERSPOSITION3_HAND2", fingersPosition3_Hand2);
        form.AddField("FINGERSPOSITION4_HAND2", fingersPosition4_Hand2);
        form.AddField("FINGERSPOSITION5_HAND2", fingersPosition5_Hand2);
        form.AddField("PALMPOSITION1",palmPosition1);
        form.AddField("PALMPOSITION2",palmPosition2);
        form.AddField("PALMVELOCITY1",palmVelocity1);
        form.AddField("PALMVELOCITY2",palmVelocity2);
        form.AddField("EXERCISENUMBER", exerciseNumber);

        WWW www = new WWW("http://localhost/sqlconnect/saveData.php",form);
        yield return www;
        UnityEngine.Debug.Log(www.text);
    }

}
