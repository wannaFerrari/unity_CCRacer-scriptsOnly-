using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class controller : MonoBehaviourPun
{
    private GameManager manager;
    private inputManager IM;
    private carEffects CarEffects;
    [HideInInspector]public bool test; //engine sound boolean

    [Header("Variables")]
    public float handBrakeFrictionMultiplier = 2f;
    public float maxRPM , minRPM;
    public float[] gears;
    public float[] gearChangeSpeed;
    public AnimationCurve enginePower;
    public int BrakePowerValue;

    public bool isOnline = true;


    [HideInInspector]public int gearNum = 1;
    [HideInInspector] public bool playPauseSmoke = false;
    [HideInInspector]public float KPH;
    [HideInInspector]public float engineRPM;
    [HideInInspector]public bool reverse = false;
    
    private GameObject wheelMeshes,wheelColliders;
    private WheelCollider[] wheels = new WheelCollider[4];
    private GameObject[] wheelMesh = new GameObject[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    public bool isFinished = false;
    public Transform spawnplace;
    public Transform camSpawnplace;
    public GameObject cam;

    public GameObject PlayerUICanvas;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera leftCam, frontCam, rightCam;

    [Header("Waypoints")]
    public WayPoint waypoints;
    public Transform currentWaypoint;
    public List<Transform> nodes = new List<Transform>();
    public int currentNode = 0;
    public int currentScore = 0;
    public int currentCheckPoint = 0;
    public bool lapClearedFlag = false;

    public List<GameObject> checkPoints= new List<GameObject>();
    public List<GameObject> spawnPlaces = new List<GameObject>();
    public List<GameObject> cameraSpawnPlaces = new List<GameObject>();
    //public float distance = Mathf.Infinity;

    [Header("Test")]
    public GameObject rpcObject; 
    public RPC_CheckLoad rpcScript;
    public bool isFirstPos = true;

    //public Rigidbody rig;

    private float smoothTime = 0.05f;

    //car tuning variables
    private float itemBrake = 0f, itemDownForce = 0f, itemAngle = 0f, itemTorque = 0f, itemWeight = 0f, itemDamper = 0f , itemGrip = 0f, itemSpring = 0f;


	private WheelFrictionCurve  forwardFriction,sidewaysFriction;
    private float radius = 6, brakPower = 0, DownForceValue = 10f, wheelsRPM, driftFactor, lastValue, horizontal, vertical, totalPower;
    private bool flag=false;
    private float angleX, angleY, angleZ;



    private void Awake() {
        // rig = this.GetComponent<Rigidbody>();
        //if(SceneManager.GetActiveScene().name == "awakeScene")return;
        if (isOnline)
        {
            if (photonView.IsMine)
            {
                //Debug.Log("isOnline");
                //waypoints = GameObject.FindGameObjectWithTag("path").GetComponent<WayPoint>();
                Transform[] arr = GameObject.FindGameObjectWithTag("checkPoints").GetComponentsInChildren<Transform>();

                for(int i =1; i< arr.Length;i++)
                {
                    if( i % 3 == 0)
                    {
                        // 3, 6, 9...
                        cameraSpawnPlaces.Add(arr[i].gameObject);
                    }
                    else if( i % 3 == 1)
                    {
                        //1 ,4  ,7 ...
                        checkPoints.Add(arr[i].gameObject);
                    }
                    else if(i % 3 == 2)
                    {
                        // 2, 5, 8 ...
                        spawnPlaces.Add(arr[i].gameObject);

                    }
                    
                }
                Debug.Log(checkPoints[0].name);
                Debug.Log(spawnPlaces[0].name);
                Debug.Log(cameraSpawnPlaces[0].name);


                //nodes = waypoints.nodes;
                getObjects();
                itemTorque = savedData.data.savedTorque;
                itemWeight = savedData.data.savedWeight;
                itemBrake = savedData.data.savedBrake;
                itemDownForce = savedData.data.savedDownforce;
                itemAngle = savedData.data.savedAngle;
                itemDamper = savedData.data.savedDamper;
                itemGrip = savedData.data.savedGrip;
                itemSpring = savedData.data.savedSpring;


                /*
                JointSpring j = new JointSpring();
                j.spring = wheels[0].suspensionSpring.spring;
                j.damper = 10000f;
                j.targetPosition = 0.3f;
                wheels[0].suspensionSpring = j;
                //wheels[0].jointspring.damper = 400f;
                Debug.Log(wheels[0].suspensionSpring.spring);*/

                SetSuspensionTuning();


                //rigidbody.mass += itemWeight;
                this.GetComponent<Rigidbody>().mass += itemWeight;
                Debug.Log(itemTorque);
                Debug.Log(rigidbody.mass);
                SetSuspensionTuning();
                //rigidbody.mass += itemWeight;
                StartCoroutine(timedLoop());
            }
        }
        else
        {
            Transform[] arr = GameObject.FindGameObjectWithTag("checkPoints").GetComponentsInChildren<Transform>();

            for (int i = 1; i < arr.Length; i++)
            {
                if (i % 3 == 0)
                {
                    // 3, 6, 9...
                    cameraSpawnPlaces.Add(arr[i].gameObject);
                }
                else if (i % 3 == 1)
                {
                    //1 ,4  ,7 ...
                    checkPoints.Add(arr[i].gameObject);
                }
                else if (i % 3 == 2)
                {
                    // 2, 5, 8 ...
                    spawnPlaces.Add(arr[i].gameObject);

                }

            }
            Debug.Log(checkPoints[0].name);
            Debug.Log(spawnPlaces[0].name);
            Debug.Log(cameraSpawnPlaces[0].name);

            getObjects();
            
            itemTorque = savedData.data.savedTorque;
            itemWeight = savedData.data.savedWeight;
            itemBrake = savedData.data.savedBrake;
            itemDownForce = savedData.data.savedDownforce;
            itemAngle = savedData.data.savedAngle;
            itemDamper = savedData.data.savedDamper;
            itemGrip = savedData.data.savedGrip;
            itemSpring = savedData.data.savedSpring;

            
            /*
            JointSpring j = new JointSpring();
            j.spring = wheels[0].suspensionSpring.spring;
            j.damper = 10000f;
            j.targetPosition = 0.3f;
            wheels[0].suspensionSpring = j;
            //wheels[0].jointspring.damper = 400f;
            Debug.Log(wheels[0].suspensionSpring.spring);*/
            
            SetSuspensionTuning();


            //rigidbody.mass += itemWeight;
            this.GetComponent<Rigidbody>().mass += itemWeight;
            Debug.Log(itemTorque);
            Debug.Log(rigidbody.mass);
            StartCoroutine(timedLoop());
        }

    }

    private void Start()
    {
         setCameras();
        if (photonView.IsMine)
        {
            rpcObject = GameObject.FindGameObjectWithTag("CheckUsers");
            rpcScript = rpcObject.GetComponent<RPC_CheckLoad>();

            //PhotonView pv = GameObject.FindGameObjectWithTag("CheckUsers").GetComponent<PhotonView>();
            PhotonView pv = rpcObject.GetComponent<PhotonView>();
            pv.RPC("UserLoaded", RpcTarget.All);
            isFirstPos = true;
            manager.isFirstPositionCheck(isFirstPos);
        }
        if (!photonView.IsMine)
        {/*
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            for(int i=0; i< scripts.Length; i++)
            {
                if (scripts[i] is GameManager) continue;
                else if (scripts[i] is PhotonView) continue;
                else if (scripts[i] is PhotonTransformView) continue;

                scripts[i].enabled = false;
                GetComponent<audio>().enabled = false;
                //GetComponent<controller>().enabled = false;
                GetComponent<AudioListener>().enabled = false;
                GetComponent<cameraController>().enabled = false;
            }*/
        }
    }

    private void Update() 
    {

        //if(SceneManager.GetActiveScene().name == "awakeScene")return;
        //Debug.Log($"last: {lastValue}, engine : {engineRPM}");
        if (photonView.IsMine)
        {
            horizontal = IM.horizontal;
            vertical = IM.vertical;
            lastValue = engineRPM;
        }
        else
        {
            //PlayerUICanvas.SetActive(false);
            /*Destroy(mainCamera);
            Destroy(leftCam);
            Destroy(frontCam);
            Destroy(rightCam);*/
            /*
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i] is GameManager) continue;
                else if (scripts[i] is PhotonView) continue;
                else if (scripts[i] is PhotonTransformView) continue;

                scripts[i].enabled = false;
                Debug.Log(scripts[i] +" ----"+ scripts[i].enabled);
                GetComponent<audio>().enabled = false;
                
                //GetComponent<controller>().enabled = false;
                GetComponent<AudioListener>().enabled = false;
                GetComponent<cameraController>().enabled = false;
            }*/
        }
         
        if (photonView.IsMine)
        {
            addDownForce();
            animateWheels();
            steerVehicle();
            calculateEnginePower();
            adjustTraction();
            
        }
        //carRatateClamp();
        if (isFinished) stopVehicle();
    }
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //calculateDistanceOfWaypoints();
        }
    }
    private void calculateEnginePower(){

        wheelRPM();

            if (vertical != 0 ){
            rigidbody.drag = 0.005f; 
            }
            if (vertical == 0){
                rigidbody.drag = 0.1f;
            }
        if (vertical < 0)
        {
            totalPower = 3.6f * (enginePower.Evaluate(engineRPM) + itemTorque) * (vertical) * 1.1f;

        }
        else
        {
            totalPower = 3.6f * (enginePower.Evaluate(engineRPM) + itemTorque) * (vertical);
        }
        


        float velocity  = 0.0f;
        if (engineRPM >= maxRPM || flag ){
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.02f);

            flag = (engineRPM >= maxRPM - 450)?  true : false;
            test = (lastValue > engineRPM) ? true : false;
        }
        else { 
            engineRPM = Mathf.SmoothDamp(engineRPM,1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity , smoothTime);
            test = false;
        }
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max
        moveVehicle();
        shifter();
        //Debug.Log($"abs: {Mathf.Abs(wheelsRPM)}, gears : {gears[gearNum]}");
        //Debug.Log($"last: {lastValue}, engine : {engineRPM}");
        //Debug.Log(lastValue+"lastValue");
        //Debug.Log(engineRPM + "engineRPM");
    }

    private void wheelRPM(){
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
        //wheelsRPM = sum / R;

        if (wheelsRPM < 0 && !reverse ){
            reverse = true;
            manager.changeGear();
        }
        else if(wheelsRPM > 0 && reverse){
            reverse = false;
            manager.changeGear();
        }
    }

    private bool checkGears(){
        if(KPH >= gearChangeSpeed[gearNum] ) return true;
        else return false;
    }

    private void shifter(){

        if(!isGrounded())return;

        if(engineRPM > maxRPM && gearNum < gears.Length-1 && !reverse && checkGears() ){
            gearNum ++;
            manager.changeGear();
            return;
        }
        if(engineRPM < minRPM && gearNum > 0){
            gearNum --;
            manager.changeGear();
        }
        
    }
 
    private bool isGrounded(){
        if(wheels[0].isGrounded &&wheels[1].isGrounded &&wheels[2].isGrounded &&wheels[3].isGrounded )
            return true;
        else
            return false;
    }

    private void moveVehicle(){

        brakeVehicle();

        for (int i = 0; i < wheels.Length; i++)
        {
            if ((KPH > 380 + (itemTorque/2)) || (wheelsRPM <= 0 && KPH > 97))
            {
                wheels[i].motorTorque = 0;
                wheels[i].brakeTorque = brakPower;
            }
            else
            {
                wheels[i].motorTorque = totalPower / 4;
                wheels[i].brakeTorque = brakPower;
            }
        }

        KPH = rigidbody.velocity.magnitude * 3.6f; //속도 계산 공식,   3.6 대신 2.237 곱하면 mph


    }

    private void brakeVehicle(){

        if (vertical < 0){
            brakPower = (KPH >= 100) ? (2500 + itemBrake) : 1000;
            if(wheelsRPM <= 0)
            {
                brakPower = 0;
            }
        }
        else if (vertical == 0 &&(KPH <= 10 || KPH >= -10)){
            brakPower = 10;
        }
        else if (vertical > 0 && wheelsRPM < 0)
        {
            brakPower = 1000;
        }
        else{
            brakPower = 0;
        }
    }

    public void stopVehicle()
    {
       
        IM.KeyboardStatus = false;
        if (wheels[0].rpm < 0) IM.vertical = 5.0f;
        else if (wheels[0].rpm == 0) IM.vertical = 0.0f;
        else IM.vertical = -5.0f;
        IM.horizontal = 0.0f;
        
        if (KPH <= 5)
        {
            IM.vertical = 0.0f;
            manager.freezeFinishedPlayers();
        }
    }
  
    private void steerVehicle(){


        //acerman steering formula
		//steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;
         
        if (horizontal > 0 ) {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            //wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            //wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + (itemAngle * 0.1f)) / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + itemAngle * 0.1f) / (radius - (1.5f / 2))) * horizontal;
            //Debug.Log(wheels[0].steerAngle + "---" + wheels[1].steerAngle);
            //                                         (앞바퀴 뒷바퀴 사이 거리) / (선회중심점 + (양바퀴 사이거리 / 2))
        } else if (horizontal < 0 ) {
            //wheels[0].steerAngle =  Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            //wheels[1].steerAngle =  Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + itemAngle * 0.1f) / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + itemAngle * 0.1f) / (radius + (1.5f / 2))) * horizontal;
            //Debug.Log(wheels[0].steerAngle + "---" + wheels[1].steerAngle);

        } else {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }

    }

    private void animateWheels ()
	{
		Vector3 wheelPosition = Vector3.zero;
		Quaternion wheelRotation = Quaternion.identity;

		for (int i = 0; i < 4; i++) {
			wheels [i].GetWorldPose (out wheelPosition, out wheelRotation);
			wheelMesh [i].transform.position = wheelPosition;
			wheelMesh [i].transform.rotation = wheelRotation;
		}
	}
   
    private void getObjects(){
        IM = GetComponent<inputManager>();
        manager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        CarEffects = GetComponent<carEffects>();
        rigidbody = GetComponent<Rigidbody>();
        wheelColliders = gameObject.transform.Find("wheelColliders").gameObject;
        wheelMeshes = gameObject.transform.Find("wheelMeshes").gameObject;
        wheels[0] = wheelColliders.transform.Find("0").gameObject.GetComponent<WheelCollider>();
        wheels[1] = wheelColliders.transform.Find("1").gameObject.GetComponent<WheelCollider>();
        wheels[2] = wheelColliders.transform.Find("2").gameObject.GetComponent<WheelCollider>();
        wheels[3] = wheelColliders.transform.Find("3").gameObject.GetComponent<WheelCollider>();

        wheelMesh[0] = wheelMeshes.transform.Find("0").gameObject;
        wheelMesh[1] = wheelMeshes.transform.Find("1").gameObject;
        wheelMesh[2] = wheelMeshes.transform.Find("2").gameObject;
        wheelMesh[3] = wheelMeshes.transform.Find("3").gameObject;

        centerOfMass = gameObject.transform.Find("mass").gameObject;
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;   
    }

    private void addDownForce(){
        if (photonView.IsMine)
        {
            rigidbody.AddForce(-transform.up * (DownForceValue + (itemDownForce * 0.2f)) * rigidbody.velocity.magnitude);
        }
    }
    
    private void adjustTraction(){
            //tine it takes to go from normal drive to drift 
        float driftSmothFactor = 0.7f * Time.deltaTime;

		if(IM.handbrake){
            sidewaysFriction = wheels[0].sidewaysFriction;
            forwardFriction = wheels[0].forwardFriction;

            float velocity = 0;
            sidewaysFriction.extremumValue =sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue =
                Mathf.SmoothDamp(forwardFriction.asymptoteValue,driftFactor * handBrakeFrictionMultiplier,ref velocity ,driftSmothFactor );

            for (int i = 0; i < 4; i++) {
                wheels [i].sidewaysFriction = sidewaysFriction;
                wheels [i].forwardFriction = forwardFriction;
            }

            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue =  1.1f;
                //extra grip for the front wheels
            for (int i = 0; i < 2; i++) {
                wheels [i].sidewaysFriction = sidewaysFriction;
                wheels [i].forwardFriction = forwardFriction;
            }
            rigidbody.AddForce(transform.forward * (KPH / 400) * 1000 );
		}
            //executed when handbrake is being held
        else{

			forwardFriction = wheels[0].forwardFriction;
			sidewaysFriction = wheels[0].sidewaysFriction;

			forwardFriction.extremumValue = forwardFriction.asymptoteValue = sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = 
                ((KPH * handBrakeFrictionMultiplier) / 300) + (1.1f + (itemGrip * 0.001f));

			for (int i = 0; i < 4; i++) {
				wheels [i].forwardFriction = forwardFriction;
				wheels [i].sidewaysFriction = sidewaysFriction;

			}
        }

            //checks the amount of slip to control the drift
		for(int i = 2;i<4 ;i++){

            WheelHit wheelHit;

            wheels[i].GetGroundHit(out wheelHit);
                //smoke
            if(wheelHit.sidewaysSlip >= 0.3f || wheelHit.sidewaysSlip <= -0.3f ||wheelHit.forwardSlip >= .3f || wheelHit.forwardSlip <= -0.3f)
                playPauseSmoke = true;
            else
                playPauseSmoke = false;
                        

			if(wheelHit.sidewaysSlip < 0 )	driftFactor = (1 + -IM.horizontal) * Mathf.Abs(wheelHit.sidewaysSlip) ;

			if(wheelHit.sidewaysSlip > 0 )	driftFactor = (1 + IM.horizontal )* Mathf.Abs(wheelHit.sidewaysSlip );
		}	
		
	}
    
	private IEnumerator timedLoop(){
		while(true){
			yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;
            
		}
	}
    public void resetCar()
    {
        if (isOnline)
        {
            if (currentCheckPoint == 0)
            {
                currentCheckPoint = 0;
            }
            else
            {
                currentCheckPoint--;
                currentScore--;
                PhotonView pv = rpcObject.GetComponent<PhotonView>();
                pv.RPC("UpdateScore", RpcTarget.All, PhotonNetwork.IsMasterClient, currentScore);

                bool buf1 = isFirstPos;
                isFirstPos = rpcScript.ReturnCurrentPosition(PhotonNetwork.IsMasterClient);
                if (buf1 != isFirstPos)
                {
                    manager.isFirstPositionCheck(isFirstPos);
                }
            }
            checkPoints[currentCheckPoint].gameObject.SetActive(true);
            Debug.Log(currentCheckPoint);
            transform.position = spawnPlaces[currentCheckPoint].transform.position;
            transform.rotation = spawnPlaces[currentCheckPoint].transform.rotation;
            mainCamera.transform.position = cameraSpawnPlaces[currentCheckPoint].transform.position;
            rigidbody.velocity = new Vector3(0, 0, 0);
        }
        else
        {


            if (currentCheckPoint == 0)
            {
                currentCheckPoint = 0;
            }
            else
            {
                currentCheckPoint--;
                currentScore--;
                PhotonView pv = rpcObject.GetComponent<PhotonView>();
                pv.RPC("UpdateScore", RpcTarget.All, PhotonNetwork.IsMasterClient, currentScore);

                /*
                bool buf1 = isFirstPos;
                isFirstPos = rpcScript.ReturnCurrentPosition(PhotonNetwork.IsMasterClient);
                if (buf1 != isFirstPos)
                {
                    manager.isFirstPositionCheck(isFirstPos);
                }*/
            }
            checkPoints[currentCheckPoint].gameObject.SetActive(true);
            Debug.Log(currentCheckPoint);
            transform.position = spawnPlaces[currentCheckPoint].transform.position;
            transform.rotation = spawnPlaces[currentCheckPoint].transform.rotation;
            mainCamera.transform.position = cameraSpawnPlaces[currentCheckPoint].transform.position;
            rigidbody.velocity = new Vector3(0, 0, 0);
        }
        /*
        transform.position = spawnplace.position;
        transform.rotation = spawnplace.rotation;
        cam.transform.position = camSpawnplace.position;
        rigidbody.velocity = new Vector3(0, 0, 0);*/
    }

    public void SetSuspensionTuning()
    {
        //List<WheelCollider> list = new List<WheelCollider>();
        for (int i = 0; i<4; i++)
        {
            JointSpring j = new JointSpring();
            j.spring = wheels[i].suspensionSpring.spring;
            j.damper = wheels[i].suspensionSpring.damper + itemDamper;
            j.targetPosition = wheels[i].suspensionSpring.targetPosition;
            wheels[i].suspensionSpring = j;
            wheels[i].suspensionDistance += itemSpring * 0.03f;
           // Debug.Log(wheels[i].suspensionSpring.damper);
        }
    }

    private void calculateDistanceOfWaypoints()
    {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            int k;
            if(currentDistance < distance)
            {
                if((i + 0) > nodes.Count )
                {
                    currentWaypoint = nodes[0];
                    distance = currentDistance;
                    //currentNode = 1000 * manager.CheckLap();
                    currentNode = 0;
                    currentScore = 1000 * manager.CheckLap();
                    /*
                    if (manager.CheckLap() == 1)
                    {
                        currentNode = 1000 * manager.CheckLap();
                    }
                    else 
                    { 
                        //currentNode = 0; 
                    }*/
                }
                else
                {
                    currentWaypoint = nodes[i + 0];
                    distance = currentDistance;
                    // Debug.Log(i);
                    currentNode = i;
                    currentScore = 1000 * manager.CheckLap() + i;

                    /*
                    k = i;
                    //if(((k - currentNode) < ( 3 ) || (k - currentNode) > (1000 - nodes.Count - 1 )) && (k - currentNode >= 0))
                    if (((k - currentNode) < (3)) && (k - currentNode > 0) || k == 0)
                    {
                        currentNode = k;
                        currentScore = currentNode;
                    }
                    //else if ((k - currentNode) >= (nodes.Count - 1 ) && (k-currentNode) <= (1000 - nodes.Count  - 1 ))
                    else
                    {
                        if(manager.CheckLap() == 0)
                        {
                            //currentNode = -nodes.Count + k;
                            currentNode = k;
                            currentScore = k - nodes.Count;
                        }
                        else
                        {
                            currentNode = k;
                            currentScore = 1000 - k;
                        }
                    }
                    */

                    //currentNode = 1000 * manager.CheckLap() + i;
                    /*
                    if (currentNode < nodes.Count-1 || currentNode > 1000)
                    {
                        currentNode = 1000 * manager.CheckLap() + i;
                    }*/
                    /*
                    if (manager.CheckLap() == 1)
                    {
                        currentNode = 1000 * manager.CheckLap() + i;
                    }
                    else
                    {
                        if (currentNode < 277)
                        {
                            currentNode = i;
                        }
                    }*/
                    //currentNode = i;
                }
            }
            
        }
    }

    public void carRatateClamp()
    {

        Quaternion z = transform.rotation;
        if (z.z < -0.2f || z.z > 0.2f) { 
        //float zf;
        z.z = Mathf.Clamp(z.z, -0.2f, 0.2f);
        //z.z = 45;
        /*if( z.z * 180 > 40f)
        {
            zf = 40;
            //Debug.Log("++++++++++++++++++++++++++++++++++++++");
        }
        else if ( z.z * 180 < -40f)
        {
            zf = -40;
            //Debug.Log("----------------------------");
        }
        else
        {
            zf = transform.eulerAngles.z;
            //Debug.Log(transform.rotation.z);
        }*/
        angleX = transform.eulerAngles.x;
        angleY = transform.eulerAngles.y;
        //transform.rotation = Quaternion.Euler(new Vector3(angleX, angleY, zf));
        // angleX = transform.eulerAngles.x;
        // angleY = transform.eulerAngles.y;
        // angleZ = transform.eulerAngles.z;
        //angleZ = Mathf.Clamp(angleZ, 0, 45);


        transform.eulerAngles = new Vector3(angleX, angleY, z.z * 180);
        }
    }

    public void setCameras()
    {
        if (photonView.IsMine)
        {
            /*
            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PhotonView>().IsMine)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            if (GameObject.FindGameObjectWithTag("frontCam").GetComponent<PhotonView>().IsMine)
            {
                frontCam = GameObject.FindGameObjectWithTag("frontCam").GetComponent<Camera>();
            }
            if (GameObject.FindGameObjectWithTag("leftCam").GetComponent<PhotonView>().IsMine)
            {
                leftCam = GameObject.FindGameObjectWithTag("leftCam").GetComponent<Camera>();
            }
            if (GameObject.FindGameObjectWithTag("rightCam").GetComponent<PhotonView>().IsMine)
            {
                rightCam = GameObject.FindGameObjectWithTag("rightCam").GetComponent<Camera>();
            }*/
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            //if(gameObject.Find("zz"))
            leftCam = GameObject.FindGameObjectWithTag("leftCam").GetComponent<Camera>();
            frontCam = GameObject.FindGameObjectWithTag("frontCam").GetComponent<Camera>();
            rightCam = GameObject.FindGameObjectWithTag("rightCam").GetComponent<Camera>();

            leftCam.enabled = false;
            frontCam.enabled = false;
            rightCam.enabled = false;
        }
        else
        {
           
        }
        /*
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        leftCam = GameObject.FindGameObjectWithTag("leftCam").GetComponent<Camera>();
        frontCam = GameObject.FindGameObjectWithTag("frontCam").GetComponent<Camera>();
        rightCam = GameObject.FindGameObjectWithTag("rightCam").GetComponent<Camera>();

        leftCam.enabled = false;
        frontCam.enabled = false;
        rightCam.enabled = false;*/

        //leftCam.GetComponent<Camera>().enabled = true;
        /*Cameras[0] = mainCamera;
        Cameras[1] = leftCam;
        Cameras[2] = frontCam;
        Cameras[3] = rightCam;*/


    }

    public void cameraChange(int num)
    {
        if (photonView.IsMine)
        {
            /*
            for (int i=0; i<Cameras.Length; i++)
            {
                if ( num == i)
                {
                    Cameras[num].enabled = true;
                }
                else
                {
                    Cameras[i].enabled = false;
                }
            }*/
            if (num == 0)
            {
                mainCamera.enabled = true;
                leftCam.enabled = false;
                frontCam.enabled = false;
                rightCam.enabled = false;
            }
            else if (num == 1)
            {
                mainCamera.enabled = false;
                leftCam.enabled = true;
                frontCam.enabled = false;
                rightCam.enabled = false;
            }
            else if (num == 2)
            {
                mainCamera.enabled = false;
                leftCam.enabled = false;
                frontCam.enabled = true;
                rightCam.enabled = false;
            }
            else if (num == 3)
            {
                mainCamera.enabled = false;
                leftCam.enabled = false;
                frontCam.enabled = false;
                rightCam.enabled = true;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isOnline)
        {
            if (gameObject.GetComponent<PhotonView>().IsMine)
            {
                if (other.gameObject.CompareTag("checkP"))
                {
                    if (other.gameObject.Equals(checkPoints[currentCheckPoint]))
                    {
                        Debug.Log(currentCheckPoint);
                        other.gameObject.SetActive(false);
                        if (currentCheckPoint == checkPoints.Count - 1)
                        {
                            currentCheckPoint = 0;
                            lapClearedFlag = true;
                            for (int i = 0; i < checkPoints.Count - 1; i++)
                            {
                                checkPoints[i].SetActive(true);
                            }

                        }
                        else if (currentCheckPoint == 0)
                        {
                            checkPoints[checkPoints.Count - 1].SetActive(true);
                            currentCheckPoint++;
                            lapClearedFlag = false;
                        }
                        else
                        {
                            currentCheckPoint++;


                        }

                        currentScore++;
                        PhotonView pv = rpcObject.GetComponent<PhotonView>();
                        pv.RPC("UpdateScore", RpcTarget.All, PhotonNetwork.IsMasterClient, currentScore);
                        /*
                        bool buf1 = isFirstPos;
                        isFirstPos = rpcScript.ReturnCurrentPosition(PhotonNetwork.IsMasterClient);
                        if (buf1 != isFirstPos)
                        {
                            manager.isFirstPositionCheck(isFirstPos);
                        }*/
                        

                    }
                    else
                    {
                        resetCar();
                    }
                }
            }
        }
        else
        {
            if (other.gameObject.Equals(checkPoints[currentCheckPoint]))
            {
                Debug.Log(currentCheckPoint);
                other.gameObject.SetActive(false);
                if (currentCheckPoint == checkPoints.Count - 1)
                {
                    currentCheckPoint = 0;
                    lapClearedFlag = true;
                    for (int i = 0; i < checkPoints.Count - 1; i++)
                    {
                        checkPoints[i].SetActive(true);
                    }

                }
                else if (currentCheckPoint == 0)
                {
                    checkPoints[checkPoints.Count - 1].SetActive(true);
                    currentCheckPoint++;
                    lapClearedFlag = false;
                }
                else
                {
                    currentCheckPoint++;


                }

                currentScore++;

            }
            else
            {
                resetCar();
            }
        }
    
        
        /*
        if (other.gameObject.CompareTag("checkP"))
        {
            if (other.gameObject.Equals(checkPoints[currentCheckPoint]))
            {
                Debug.Log(currentCheckPoint);
                other.gameObject.SetActive(false);
                if (currentCheckPoint == checkPoints.Count - 1)
                {
                    currentCheckPoint = 0;
                    lapClearedFlag = true;
                    for (int i = 0; i < checkPoints.Count - 1; i++)
                    {
                        checkPoints[i].SetActive(true);
                    }

                }
                else if (currentCheckPoint == 0)
                {
                    checkPoints[checkPoints.Count - 1].SetActive(true);
                    currentCheckPoint++;
                    lapClearedFlag = false;
                }
                else
                {
                    currentCheckPoint++;


                }

                currentScore++;

            }
            else
            {
                resetCar();
            }
        }
        */
        //-------------------------------------------------------------
        /*
        if (other.gameObject.Equals(checkPoints[currentCheckPoint]))
        {
            Debug.Log(currentCheckPoint);
            other.gameObject.SetActive(false);
            if(currentCheckPoint == checkPoints.Count -1)
            {
                currentCheckPoint = 0;
                lapClearedFlag = true;
                for (int i = 0; i<checkPoints.Count-1; i++)
                {
                    checkPoints[i].SetActive(true);
                }
                
            }
            else if(currentCheckPoint == 0)
            {
                checkPoints[checkPoints.Count-1].SetActive(true);
                currentCheckPoint++;
                lapClearedFlag = false;
            }
            else
            {
                currentCheckPoint++;
                

            }
            
            currentScore++;

        }
        else
        {
            
            if (other.gameObject.CompareTag("Finish"))
            {

            }
            else if (other.gameObject.CompareTag("checkP"))
            {
                resetCar();
            }
        }*/
    }

    public bool ReturnIsReadyToNextLap()
    {
        return lapClearedFlag;
        /*
       if(currentCheckPoint == checkPoints.Count - 1)
        {
            return true;
        }
        else
        {
            return false;
        }*/
    }

}
