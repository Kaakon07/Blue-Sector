using UnityEngine;

public class AdvancedOcean : MonoBehaviour
{
    private const int NB_WAVE = 5;
    private const int NB_INTERACTIONS = 64 / 8;

    public Material ocean;
    public Light sun;

    private static readonly Wave[] waves =
    {
        //new Wave(99, 1.0f, 1.4f, 0.9f, new Vector2(1.0f,  0.2f)),
        //new Wave(60, 1.2f, 0.8f, 0.5f, new Vector2(1.0f,  3.0f)),
        //new Wave(20, 3.5f, 0.4f, 0.8f, new Vector2(2.0f,  4.0f)),
        //new Wave(30, 2.0f, 0.4f, 0.4f, new Vector2(-1.0f, 0.0f)),
        //new Wave(10, 3.0f, 0.05f, 0.9f,new Vector2(-1.0f, 1.2f))
        
        //new Wave(99, 1.0f, .4f * 1.4f, 0.9f, new Vector2(1.0f,  0.2f)),
        //new Wave(60, 1.2f, .4f * 0.8f, 0.5f, new Vector2(1.0f,  3.0f)),
        //new Wave(20, 3.5f, .4f * 0.4f, 0.8f, new Vector2(2.0f,  4.0f)),
        //new Wave(30, 2.0f, .4f * 0.4f, 0.4f, new Vector2(-1.0f, 0.0f)),
        //new Wave(10, 3.0f, .4f * 0.05f, 0.9f,new Vector2(-1.0f, 1.2f))

/*        new Wave(99, 1.0f, .4f * 10.4f, 0.9f, new Vector2(1.0f,  0.2f)),
        new Wave(60, 1.2f, .4f * 0.8f, 0.5f, new Vector2(1.0f,  3.0f)),
        new Wave(20, 3.5f, .4f * 0.4f, 0.8f, new Vector2(2.0f,  4.0f)),
        new Wave(30, 2.0f, .4f * 8.4f, 0.4f, new Vector2(-1.0f, 0.0f)),
        new Wave(10, 3.0f, .4f * 0.05f, 0.9f,new Vector2(-1.0f, 1.2f))*/

        new Wave(99, 1.0f, .4f * 0.07f, 0.9f, new Vector2(1.0f,  0.2f)),
        new Wave(60, 1.2f, .4f * 0.05f, 0.5f, new Vector2(1.0f,  3.0f)),
        new Wave(20, 3.5f, .4f * 0.065f, 0.8f, new Vector2(2.0f,  4.0f)),
        new Wave(30, 2.0f, .4f * 0.05f, 0.4f, new Vector2(-1.0f, 0.0f)),
        new Wave(10, 3.0f, .4f * 0.055f, 0.9f,new Vector2(-1.0f, 1.2f))
    };
    private readonly Vector4[] interactions = new Vector4[NB_INTERACTIONS];
    private int interaction_id = 0;

    void Awake()
    {
        Vector4[] v_waves = new Vector4[NB_WAVE];
        Vector4[] v_waves_dir = new Vector4[NB_WAVE];
        for (int i = 0; i < NB_WAVE; i++)
        {
            v_waves[i] = new Vector4(waves[i].frequency, waves[i].amplitude, waves[i].phase, waves[i].sharpness);
            v_waves_dir[i] = new Vector4(waves[i].direction.x, waves[i].direction.y, 0, 0);
        }

        ocean.SetVectorArray("waves_p", v_waves);
        ocean.SetVectorArray("waves_d", v_waves_dir);

        for (int i = 0; i < NB_INTERACTIONS; i++)
            interactions[i].w = 500.0F;
        ocean.SetVectorArray("interactions", interactions);
        ocean.SetVector("world_light_dir", -sun.transform.forward);
        ocean.SetVector("sun_color", new Vector4(sun.color.r, sun.color.g, sun.color.b, 0.0F));
    }

    public void RegisterInteraction(Vector3 pos, float strength)
    {
        interactions[interaction_id].x = pos.x;
        interactions[interaction_id].y = pos.z;
        interactions[interaction_id].z = strength;
        interactions[interaction_id].w = Time.time;
        ocean.SetVectorArray("interactions", interactions);
        interaction_id = (interaction_id + 1) % NB_INTERACTIONS;
    }

    public float GetWaterHeight(Vector3 p)
    {
        float height = 0;
        for (int i = 0; i < NB_WAVE; i++)
            height += waves[i].amplitude * Mathf.Sin(Vector2.Dot(waves[i].direction, new Vector2(p.x, p.z)) * waves[i].frequency + Time.time * waves[i].phase);
        return height;
    }

    private readonly struct Wave
    {
        public float waveLength { get; }
        public float speed { get; }
        public float amplitude { get; }
        public float sharpness { get; }
        public float frequency { get; }
        public float phase { get; }
        public Vector2 direction { get; }

        public Wave(float waveLength, float speed, float amplitude, float sharpness, Vector2 direction)
        {
            this.waveLength = waveLength;
            this.speed = speed;
            this.amplitude = amplitude;
            this.sharpness = sharpness;
            this.direction = direction.normalized;
            frequency = (2 * Mathf.PI) / waveLength;
            phase = frequency * speed;
        }
    }
}
