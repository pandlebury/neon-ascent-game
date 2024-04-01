using UnityEngine;

public class ParticleLighting : MonoBehaviour
{
    public Light spotlight; // Reference to the spotlight
    public float maxIntensity = 1.0f; // Maximum intensity of the spotlight effect
    public float maxDistance = 5.0f; // Maximum distance from spotlight for full intensity

    public float destroyDelay = 5.0f;

    private ParticleSystem particles; // Reference to the particle system component

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        
        Invoke("DestroyGameObject", destroyDelay);
    }

    void Update()
    {
        if (spotlight == null || particles == null)
            return;

        // Calculate the distance and angle between each particle and the spotlight
        ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[particles.main.maxParticles];
        int numParticlesAlive = particles.GetParticles(particleArray);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 particleWorldPos = particleArray[i].position + transform.position;
            Vector3 directionToSpotlight = (spotlight.transform.position - particleWorldPos).normalized;
            float distanceToSpotlight = Vector3.Distance(particleWorldPos, spotlight.transform.position);
            float angleToSpotlight = Vector3.Angle(spotlight.transform.forward, directionToSpotlight);

            // Calculate intensity based on distance and angle
            float intensity = Mathf.Clamp01(1 - distanceToSpotlight / maxDistance) * Mathf.Clamp01(1 - angleToSpotlight / spotlight.spotAngle) * maxIntensity;

            // Apply intensity to particle color
            Color particleColor = particleArray[i].startColor;
            particleColor.a = intensity;
            particleArray[i].startColor = particleColor;
        }

        // Update particle system
        particles.SetParticles(particleArray, numParticlesAlive);
    }
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}