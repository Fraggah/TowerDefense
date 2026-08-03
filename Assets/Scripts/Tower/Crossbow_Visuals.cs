using System.Collections;
using UnityEngine;

public class Crossbow_Visuals : MonoBehaviour
{
    private Enemy myEnemy;

    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualDuration = .1f;

    [Header("Glowing Visuals")]
    [SerializeField] private MeshRenderer meshRenderer;
    private Material material;

    [Space]
    private float currentIntensity;
    [SerializeField] private float maxIntensity = 150;

    [Space]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Rotor Visuals")]
    [SerializeField] private Transform rotor;
    [SerializeField] private Transform rotorUnloaded;
    [SerializeField] private Transform rotorLoaded;


    [Header("Front Glow String")]
    [SerializeField] private LineRenderer string_FL;
    [SerializeField] private LineRenderer string_FR;

    [Space]

    [SerializeField] private Transform startPoint_FL;
    [SerializeField] private Transform startPoint_FR;
    [SerializeField] private Transform endPoint_FL;
    [SerializeField] private Transform endPoint_FR;

    [Header("Back Glow String")]
    [SerializeField] private LineRenderer string_BL;
    [SerializeField] private LineRenderer string_BR;

    [Space]

    [SerializeField] private Transform startPoint_BL;
    [SerializeField] private Transform startPoint_BR;
    [SerializeField] private Transform endPoint_BL;
    [SerializeField] private Transform endPoint_BR;

    [SerializeField] private LineRenderer[] lineRenderers;

    private void Awake()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;

        UpdateMaterialsOnLineRenderers();
        StartCoroutine(ChangeEmission(1));
    }

    private void UpdateMaterialsOnLineRenderers()
    {
        foreach (var lr in lineRenderers)
        {
            lr.material = material;
        }
    }



    private void Update()
    {
        UpdateEmissionColor();
        UpdateStrings();

        UpdateAttackVisualsIfNeeded();
    }

    private void UpdateAttackVisualsIfNeeded()
    {
        if (attackVisuals.enabled && myEnemy != null)
        {
            attackVisuals.SetPosition(1, myEnemy.CenterPoint());
        }
    }

    private void UpdateStrings()
    {
        UpdateStringVisual(string_FL, startPoint_FL, endPoint_FL);
        UpdateStringVisual(string_FR, startPoint_FR, endPoint_FR);
        UpdateStringVisual(string_BL, startPoint_BL, endPoint_BL);
        UpdateStringVisual(string_BR, startPoint_BR, endPoint_BR);
    }

    private void UpdateEmissionColor()
    {
        Color emissionColor = Color.Lerp(startColor, endColor, currentIntensity / maxIntensity);

        emissionColor = emissionColor * Mathf.LinearToGammaSpace(currentIntensity);

        material.SetColor("_EmissionColor", emissionColor);
    }

    public void PlayReloadVFX(float duration)
    {
        float newDuration = duration / 2;

        StartCoroutine(ChangeEmission(newDuration));
        StartCoroutine(UpdateRotorPosition(newDuration));
    }
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        StartCoroutine(VFXCoroutine(startPoint, endPoint, newEnemy));
    }

    private IEnumerator VFXCoroutine(Vector3 startPoint, Vector3 endPoint, Enemy newEnemy)
    {
        myEnemy = newEnemy;

        attackVisuals.enabled = true;
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualDuration);

        attackVisuals.enabled = false;
    }

    private IEnumerator ChangeEmission(float duration)
    {
        float startTime = Time.time;
        float startIntensity = 0;

        while (Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            currentIntensity = Mathf.Lerp(startIntensity, maxIntensity, tValue);
            yield return null;
        }

        currentIntensity = maxIntensity;
    }

    private IEnumerator UpdateRotorPosition(float duration)
    {
        float startTime = Time.time;

        while(Time.time - startTime < duration)
        {
            float tValue = (Time.time - startTime) / duration;
            rotor.position = Vector3.Lerp(rotorUnloaded.position, rotorLoaded.position, tValue);
            yield return null;
        }

        rotor.position = rotorLoaded.position;
    }

    private void UpdateStringVisual(LineRenderer lineRenderer, Transform startPoint, Transform endPoint)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
