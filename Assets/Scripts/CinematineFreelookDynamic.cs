using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CinematineFreelookDynamic : MonoBehaviour
{
    private CinemachineFreeLook cinemachineFreeLook;
    bool airCamera;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private float switchDuration = 1.5f;

    public float TopOrbitHeightGround = 0.63f;
    public float MiddleOrbitHeightGround = 0.4f;
    public float BottomOrbitHeightGround = 0f;

    public float TopOrbitRadiusGround = 0.8f;
    public float MiddleOrbitRadiusGround = 1f;
    public float BottomOrbitRadiusGround = 0.8f;

    public float TopOrbitHeightAir = 1f;
    public float MiddleOrbitHeightAir = 0.2f;
    public float BottomOrbitHeightAir = -0.61f;

    public float TopOrbitRadiusAir = 0.8f;
    public float MiddleOrbitRadiusAir = 1.2f;
    public float BottomOrbitRadiusAir = 0.9f;

    private IEnumerator SwitchToAirCamera()
    {
        float elapsed = 0.0f;
        while (elapsed < switchDuration)
        {
            cinemachineFreeLook.m_Orbits[0].m_Height = Mathf.Lerp(TopOrbitHeightGround, TopOrbitHeightAir, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[1].m_Height = Mathf.Lerp(MiddleOrbitHeightGround, MiddleOrbitHeightAir, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[2].m_Height = Mathf.Lerp(BottomOrbitHeightGround, BottomOrbitHeightAir, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[0].m_Radius = Mathf.Lerp(TopOrbitRadiusGround, TopOrbitRadiusAir, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[1].m_Radius = Mathf.Lerp(MiddleOrbitRadiusGround, MiddleOrbitRadiusAir, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[2].m_Radius = Mathf.Lerp(BottomOrbitRadiusGround, BottomOrbitRadiusAir, elapsed / switchDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }
        cinemachineFreeLook.m_Orbits[0].m_Height = TopOrbitHeightAir;
        cinemachineFreeLook.m_Orbits[1].m_Height = MiddleOrbitHeightAir;
        cinemachineFreeLook.m_Orbits[2].m_Height = BottomOrbitHeightAir;
        cinemachineFreeLook.m_Orbits[0].m_Radius = TopOrbitRadiusAir;
        cinemachineFreeLook.m_Orbits[1].m_Radius = MiddleOrbitRadiusAir;
        cinemachineFreeLook.m_Orbits[2].m_Radius = BottomOrbitRadiusAir;

        airCamera = true;
    }

    private IEnumerator SwitchToGroundCamera()
    {
        float elapsed = 0.0f;
        while (elapsed < switchDuration)
        {
            cinemachineFreeLook.m_Orbits[0].m_Height = Mathf.Lerp(TopOrbitHeightAir, TopOrbitHeightGround, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[1].m_Height = Mathf.Lerp(MiddleOrbitHeightAir, MiddleOrbitHeightGround, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[2].m_Height = Mathf.Lerp(BottomOrbitHeightAir, BottomOrbitHeightGround, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[0].m_Radius = Mathf.Lerp(TopOrbitRadiusAir, TopOrbitRadiusGround, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[1].m_Radius = Mathf.Lerp(MiddleOrbitRadiusAir, MiddleOrbitRadiusGround, elapsed / switchDuration);
            cinemachineFreeLook.m_Orbits[2].m_Radius = Mathf.Lerp(BottomOrbitRadiusAir, BottomOrbitRadiusGround, elapsed / switchDuration);

            elapsed += Time.deltaTime;
            yield return null;
        }
        cinemachineFreeLook.m_Orbits[0].m_Height = TopOrbitHeightGround;
        cinemachineFreeLook.m_Orbits[1].m_Height = MiddleOrbitHeightGround;
        cinemachineFreeLook.m_Orbits[2].m_Height = BottomOrbitHeightGround;
        cinemachineFreeLook.m_Orbits[0].m_Radius = TopOrbitRadiusGround;
        cinemachineFreeLook.m_Orbits[1].m_Radius = MiddleOrbitRadiusGround;
        cinemachineFreeLook.m_Orbits[2].m_Radius = BottomOrbitRadiusGround;

        airCamera = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.inTheAir && !airCamera)
        {
            StartCoroutine(SwitchToAirCamera());
        }
        else if (!playerMovement.inTheAir && airCamera)
        {
            StartCoroutine(SwitchToGroundCamera());
        }
    }
}
