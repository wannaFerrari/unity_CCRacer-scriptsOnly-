using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour
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

    //public Rigidbody rig;

    private float smoothTime = 0.05f;


	private WheelFrictionCurve  forwardFriction,sidewaysFriction;
    private float radius = 6, brakPower = 0, DownForceValue = 10f, angleItem = 0f,itemTorque = 0f, itemWeight = 0f, wheelsRPM ,driftFactor, lastValue ,horizontal , vertical,totalPower;
    private bool flag=false;
    private float angleX, angleY, angleZ;



    private void Awake() {
       // rig = this.GetComponent<Rigidbody>();
        //if(SceneManager.GetActiveScene().name == "awakeScene")return;
        getObjects();
        itemTorque = savedData.data.savedTorque;
        itemWeight = savedData.data.savedWeight;
        rigidbody.mass += itemWeight;
        //this.GetComponent<Rigidbody>().mass += itemWeight;
        Debug.Log(itemTorque);
        //Debug.Log(rigidbody.mass);
        StartCoroutine(timedLoop());

    }

    private void Update() {

        //if(SceneManager.GetActiveScene().name == "awakeScene")return;
        //Debug.Log($"last: {lastValue}, engine : {engineRPM}");

        horizontal = IM.horizontal;
        vertical = IM.vertical;
        lastValue = engineRPM;

        addDownForce();
        animateWheels();
        steerVehicle();
        calculateEnginePower();
        adjustTraction();
        //carRatateClamp();
        if (isFinished) stopVehicle();
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
            brakPower =(KPH >= 100)? BrakePowerValue : 1000;
            if(wheelsRPM < 0)
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
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + (angleItem * 0.1f)) / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + angleItem * 0.1f) / (radius - (1.5f / 2))) * horizontal;
            //Debug.Log(wheels[0].steerAngle);
            //                                         (앞바퀴 뒷바퀴 사이 거리) / (선회중심점 + (양바퀴 사이거리 / 2))
        } else if (horizontal < 0 ) {
            //wheels[0].steerAngle =  Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            //wheels[1].steerAngle =  Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + angleItem * 0.1f) / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan((2.55f + angleItem * 0.1f) / (radius + (1.5f / 2))) * horizontal;

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
        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude );
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
            rigidbody.AddForce(transform.forward * (KPH / 400) * 10000 );
		}
            //executed when handbrake is being held
        else{

			forwardFriction = wheels[0].forwardFriction;
			sidewaysFriction = wheels[0].sidewaysFriction;

			forwardFriction.extremumValue = forwardFriction.asymptoteValue = sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = 
                ((KPH * handBrakeFrictionMultiplier) / 300) + 1.1f;

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
        transform.position = spawnplace.position;
        transform.rotation = spawnplace.rotation;
        cam.transform.position = camSpawnplace.position;
        rigidbody.velocity = new Vector3(0, 0, 0);
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

}
