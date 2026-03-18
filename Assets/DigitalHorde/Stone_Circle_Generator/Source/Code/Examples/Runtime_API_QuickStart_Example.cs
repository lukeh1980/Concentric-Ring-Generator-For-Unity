using DigitalHorde.ConcentricRings.StoneCircles;
using System.Collections.Generic;
using UnityEngine;

public class Runtime_API_QuickStart_Example : MonoBehaviour {

    /// <summary>
    /// In this quick start example we will generate a simple stone circle and use the API to manage the generated stone circle.
    /// When the game starts a circle will be generated, to refresh and/or regenerate the circle use the two Context Menu buttons.
    /// Right-click the component and click "Refresh Stone Circle" or "Regenerate Stone Circle"
    /// </summary>

    // Define prefabs that can be used as endpoints:
    [Header("End Point Prefabs:")]
    [SerializeField] private List<GameObject> m_RandomEndpointList = new List<GameObject>();

    // Define end point ranges:
    [Header("Random End Point Ranges:")]
    [SerializeField] private Vector2Int m_NumberOfRingsToGenerateRange = new Vector2Int(1, 4);
    [SerializeField] private Vector2Int m_NumberOfEndpointsRange = new Vector2Int(10, 15);
    [SerializeField] private Vector2 m_EndpointRadiusRange = new Vector2(15, 30);

    // Set global variables to hold stone circle data to make it easier to manage in this example:
    private StoneCircle_Manager m_GeneratedStoneCircle;
    private List<StoneCircle_Ring> m_StoneCircleRings = new List<StoneCircle_Ring>();

    private void Start() {

        // Generate a circle when the game starts:
        RegenerateStoneCircle();

    }

    // This method will create a stone circle at a Vector3 position, returning a StoneCircle_Manager that can be called from other scripts. The method uses random values for enpoints and distance as defined by the ranges above.
    public StoneCircle_Manager GenerateRandomStoneCircle(Vector3 position) {

        // Clear out the global variables each time we instantiate a new circle:
        m_GeneratedStoneCircle = null;
        m_StoneCircleRings.Clear();

        // Instantiate a StoneCircle_Manager object:
        StoneCircle_Manager scm = StoneCircleGenerator_API.GenerateStoneCircle(position, "My Stone Circle");

        // Add a random number of rings using the prefabs and ranges specified above:
        int randomRingCount = UnityEngine.Random.Range(m_NumberOfRingsToGenerateRange.x, m_NumberOfRingsToGenerateRange.y);
        for (int i = 0; i < randomRingCount; i++) {

            StoneCircle_Ring ring = StoneCircleGenerator_API.AddRing(scm, UnityEngine.Random.Range(m_NumberOfEndpointsRange.x, m_NumberOfEndpointsRange.y), UnityEngine.Random.Range(m_EndpointRadiusRange.x, m_EndpointRadiusRange.y), m_RandomEndpointList); // add ring with number of endpoints and radius specified.
            m_StoneCircleRings.Add(ring);

        }
        
        return scm;

    }

    // Method for the context menu that can be used to regenerate a stone circle:
    [ContextMenu("Regenerate Stone Circle")]
    public void RegenerateStoneCircle() {

        // Destroy the circle if one has already been instantiated:
        if (m_GeneratedStoneCircle != null) { DestroyImmediate(m_GeneratedStoneCircle.gameObject); }

        // Instantiate a new stone circle, to do this we'll use the GenerateRandomStoneCircle method defined below, the stone circle will be generated at the location of this transform:
        m_GeneratedStoneCircle = GenerateRandomStoneCircle(this.gameObject.transform.position);

        // API calls can be made to manage the circle using the static class StoneCircleGenerator_API, let's do that and snap the stone circle to the terrain:
        StoneCircleGenerator_API.SnapCircleToTerrain(m_GeneratedStoneCircle, true);

    }

    // Method to refresh the circle with new random values:
    [ContextMenu("Refresh Stone Circle")]
    public void RefreshStoneCircle() {

        // Update ring endpoints by referencing each ring:
        for (int i = 0; i < m_StoneCircleRings.Count; i++) {

            StoneCircleGenerator_API.NumberOfEndPoints(m_StoneCircleRings[i], UnityEngine.Random.Range(m_NumberOfEndpointsRange.x, m_NumberOfEndpointsRange.y));
            StoneCircleGenerator_API.Radius(m_StoneCircleRings[i], UnityEngine.Random.Range(m_EndpointRadiusRange.x, m_EndpointRadiusRange.y));

        }

    }

}
