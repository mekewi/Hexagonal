using TMPro;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Renderer hexRenderer;
    [SerializeField]
    public Coordinates hexCorrdinates;
    public TextMeshPro CordinateText;
    public TextMeshPro TimerText;
    public Color HexColor;
    public ParticleSystem particleSystemEffect;
    float timeToMovePosition;
    Vector3 startPosition;
    public Vector3 target;
    float timeToReachTarget = 1f;
    bool StartChangePosition;
    [SerializeField]
    LevelSettings levelSettings;
    public IExplode explode;
    public void SetHexData(Color color,Coordinates cordinates) {
        HexColor = color;
        hexRenderer.material.color = color;
        hexCorrdinates = cordinates;
        UpdateText();
        startPosition = transform.position;
        Debug.Log(hexRenderer.bounds.size);
    }
    public void UpdateText() {
        if (levelSettings.showCoordinates)
        {
            CordinateText.text = hexCorrdinates.q + "|" + hexCorrdinates.r + "|" + hexCorrdinates.s;
        }
    }
    public void DestroyHex() {
        if (particleSystemEffect != null)
        {
            var main = particleSystemEffect.main;
            main.startColor = HexColor;
            particleSystemEffect.Play();
        }
        hexRenderer.gameObject.SetActive(false);
        startPosition = new Vector3(transform.position.x, 23, transform.position.z);
        timeToMovePosition = 0;
        explode.ISExploded = true;
    }
    public void HexMovedEvent() {
        explode.HexMoved();
    }
    public void BringHexToLife() {
        hexRenderer.gameObject.SetActive(true);
        UpdateText();
        Point newPoint = Layout.Instance.HexToPixel(hexCorrdinates);
        target = new Vector3((float)newPoint.x, (float)newPoint.y, 0);
        StartChangePosition = true;
        explode.UpdateText();
        hexRenderer.transform.rotation = Quaternion.Euler(0, 0, (float)levelSettings.orientationType);
    }
    void Update()
    {
        if (StartChangePosition)
        {
            timeToMovePosition += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, target, timeToMovePosition);
            if (timeToMovePosition >= timeToReachTarget)
            {
                startPosition = target;
                StartChangePosition = false;
            }
        }
    }

}
