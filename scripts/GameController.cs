using System.Collections;

public class GameController: MonoBehaviour {
    public Transform tool;
    public static  float outOfBodeyCameraSpeed = 5.0f;
    public static  bool isFirstPerson          = true;
    public static  bool isThirdPerson          = false;
    public static  bool isCameraFading         = false;
    public static  bool isTrackpadPressed      = false;

    public static int maxNumberOfEnamys = 20;
    public static int currentNumberOfEnamys = 0;

    public static int score = 0;


    public static Vector3 current_CamForward;
    public static Vector3 current_CamRight;


    public Transform mainCharacter;
    public Transform vrHeadset;

    // Update is called once per frame
    void Update () {

        //Switching out layers
        if( isCameraFading)//Om vi �r mellan tv� steg
        {

            //S� ska inte v�ran spelare kuunna interagera med fysiska object
            SetLayerRecursively(mainCharacter.gameObject, "Disabeled");
            //Om n�gon h�ller i pilb�gen s� ska den g�mas och f�r pss under fade
            if (tool.parent != null)
            {
                SetLayerRecursively(tool.gameObject, "Hidden/Disabeled");

            }
            //Vi g�mer v�ra kontrollers under fade
            HideLayer("Ignore Raycast");

        }
        else if (!isFirstPerson) //Om vi �r i tredjepersion
        {
            //V�ran spelare ska nu interagera med alla fysika element
            SetLayerRecursively(mainCharacter.gameObject,"Default");
            //Vi ska inte kunna anv�nda pilb�gen i tredjepersion och h�ller vi i den s� ska den g�mas
            if (tool.parent != null) {
                 SetLayerRecursively(tool.gameObject, "Hidden/Disabeled");
                
            }
            HideLayer("Ignore Raycast");
        }
        else //Om vi �r i f�rstapersio (Default)
        {
            //S� ska v�ran spelare vara g�md och inte interagera med fysiska object
            SetLayerRecursively(mainCharacter.gameObject, "Hidden/Disabeled");
            SetLayerRecursively(tool.gameObject, "Default");
            ShowLayer("Ignore Raycast");

            //Make Caracter follow vr-head
            mainCharacter.position = new Vector3(vrHeadset.position.x, 0.5f, vrHeadset.position.z);
            //Make caracter follow vr-gase
            Vector3 puppitRotation = mainCharacter.eulerAngles;
            puppitRotation.y = Camera.main.transform.eulerAngles.y;
            mainCharacter.eulerAngles = puppitRotation;

        }
       

    }


    public static void changePerspective()
    {
        isFirstPerson  = !isFirstPerson;
        isCameraFading = true;
    }

    void SetLayerRecursively(GameObject obj, string layerName)
    {
        int newLayer = LayerMask.NameToLayer(layerName);
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, layerName);
        }
    }


    // Turn on the bit using an OR operation:
    private static void ShowLayer(string name)
    {
       Camera.main.cullingMask |= 1 << LayerMask.NameToLayer(name);
    }

    // Turn off the bit using an AND operation with the complement of the shifted int:
    private static void HideLayer(string name)
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(name));
    }

    // Toggle the bit using a XOR operation:
    private static void ToggleLayer(string name)
    {
         Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer(name);
    }

}            