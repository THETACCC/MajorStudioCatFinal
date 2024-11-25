using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    //effects
    private bool starteffect;
    private float speed = 20f;


    //Disappear
    private float disappearTimer;
    private Color textcolor;


    //effects
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private Vector3 moveVector;
    //random rotation
    private float rotationspeed = 10f;
    private float randomtarget = 0f;
    private RectTransform m_transform;


    //Sorting
    private static int sortingorder;

    private Camera _camera;
    private TextMeshPro textmesh;
    private void Awake()
    {
        m_transform = gameObject.GetComponent<RectTransform>();
        textmesh = transform.GetComponent<TextMeshPro>();
        _camera = Camera.main;
        textcolor = Color.white;
    }

    void LateUpdate()
    {
        // transform.forward = _camera.transform.forward;
    }


    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {

        Transform damagePopupTransform = Instantiate(GameAssets.i.Damagepopup, position, Quaternion.identity);
        DamagePopup m_Damagepopup = damagePopupTransform.GetComponent<DamagePopup>();

        m_Damagepopup.Setup(damageAmount, isCriticalHit);
        return m_Damagepopup;
    }




    public void Setup(int damageAmount, bool isCriticalHit)
    {

        textmesh.SetText(damageAmount.ToString());


        if (!isCriticalHit)
        {
            // not a critical hit

            textmesh.fontSize = 8;


        }
        else
        {
            //is critical hit
            textmesh.fontSize = 8;



        }



        textmesh.color = textcolor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingorder++;
        textmesh.sortingOrder = sortingorder;
        randomtarget = Random.Range(-45, 45);




        float randomX = Random.Range(-0.5f, 0.5f);
        float randomy = Random.Range(-1, 1);

        //Make sure the text is aligned with the position
        randomtarget = randomtarget * Mathf.Sign(randomX);
        starteffect = true;
        moveVector = new Vector3(randomX, randomy) * 4f;
    }


    private void Update()
    {
        //float moveYSpeed = 2f;
        m_transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * -1f * Time.deltaTime;
        Vector3 targetrotation = new Vector3(0, 0, randomtarget);

        //Rotate the Text
        m_transform.rotation = Quaternion.Lerp(m_transform.rotation, Quaternion.Euler(targetrotation), Time.deltaTime * rotationspeed);


        /*
        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            //First half of the popup
            float increaseScaleAmount = 1f;
            transform.localScale = Vector3.Lerp( transform.localScale, Vector3.one*increaseScaleAmount * Time.deltaTime, Time.deltaTime);
            //transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;


        }
        else
        {
            //second half
            float increaseScaleAmount = -0.5f;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * increaseScaleAmount * Time.deltaTime, Time.deltaTime);
        }
        */
        if (starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1.5f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 1f, Time.deltaTime * speed);
            m_transform.localScale = newScale;



            if (newScale.x >= 1.4f)
            {
                starteffect = false;
            }
        }
        else if (!starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 2f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 2f, Time.deltaTime * speed);
            m_transform.localScale = newScale;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //Start disappearing
            float disappearspeed = 3f;
            textcolor.a -= disappearspeed * Time.deltaTime;
            textmesh.color = textcolor;
            if (textcolor.a < 0)
            {
                Destroy(gameObject);
            }
        }

    }


}
