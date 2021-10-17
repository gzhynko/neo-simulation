using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NEOSimulation.Components;
using NEOSimulation.Components.Rendering;
using NEOSimulation.Components.Input;
using NEOSimulation.Components.Orbital;
using NEOSimulation.Entities;
using NEOSimulation.Types;
using Newtonsoft.Json;
using Nez;
using Color = Microsoft.Xna.Framework.Color;
using Input = Nez.Input;

namespace NEOSimulation
{
    public class MainScene : Scene
    {
        public ArcBallCamera ArcCamera;
        public Entity CameraEntity;
        public SelectedBody SelectedBodyManager;
        public TimeManager TimeManager;
        
        public static MainScene Instance;

        public bool EarthOnlyView;
        public bool InputBlocked;

        public CelestialBody[] BodyArray;
        
        public BasicEffect BasicEffect;
        
        public override void Initialize()
        {
            base.Initialize();
            SetDesignResolution(1920, 1080, SceneResolutionPolicy.NoBorderPixelPerfect);
            ClearColor = Color.Black;

            Content.RootDirectory = "Content";
            Instance = this;

            CameraEntity = CreateEntity("arccamera");
            ArcCamera = CameraEntity.AddComponent(new ArcBallCamera(Vector3.Zero, 0f, 0f, 0f, (float)Math.PI, 500f, 2f, 20000f));
            CameraEntity.AddComponent<CameraControl>();
            
            var debugAxesEntity = CreateEntity("debugAxes");
            debugAxesEntity.AddComponent<DebugAxes>();

            BodyArray = LoadData().ToArray();
            
            var timeManagerEntity = CreateEntity("timeManager");
            TimeManager = timeManagerEntity.AddComponent<TimeManager>();
            
            var selectedObjectEntity = CreateEntity("selectedObject");
            SelectedBodyManager = selectedObjectEntity.AddComponent<SelectedBody>();

            var uiEntity = CreateEntity("ui");
            uiEntity.AddComponent<Ui>();

            BasicEffect = Core.Content.LoadMonoGameEffect<BasicEffect>();
        }

        public Body GetBodyByName(string name)
        {
            return FindEntity(name).GetComponent<Body>();
        }

        private List<CelestialBody> LoadData()
        {
            // load up planetary data
            var json = File.ReadAllText("Content/planet-orbits.json");
            var planetaryOrbitData = JsonConvert.DeserializeObject<RawPlanetaryOrbitData[]>(json);
            Insist.IsNotNull(planetaryOrbitData, "Planetary orbit data cannot be null");
            var j2000 = new DateTime(2000, 1, 1, 12, 0, 0);
            var mjdBase = new DateTime(1858, 11, 17, 0, 0, 0);

            var planets = new List<CelestialBody>();
            foreach (var body in planetaryOrbitData)
            {
                var entity = AddEntity(new CelestialBody
                {
                    Type = BodyType.Planet,
                    Name = body.Name.Trim(),
                    ArgumentOfPerihelion = body.Peri,
                    AscendingNode = body.Node,
                    Eccentricity = body.E,
                    Epoch = body.Epoch,
                    Inclination = body.I,
                    MeanAnomaly = body.M,
                    MeanMotion = body.MDot,
                    SemiMajorAxis = body.A, 
                    Color = ColorTranslator.FromHtml(body.HtmlColor),
                    DiameterKm = body.Diameter,
                    OsculationDate = j2000
                });

                entity.AddComponent<Orbit>();
                entity.AddComponent<Body>();
                entity.AddComponent<BodyName>();
                
                planets.Add(entity);
            }
            
            // load up NEO data
            var csv = new CsvReader(File.OpenText("Content/neo-orbits.csv"), CultureInfo.InvariantCulture);
            var neoOrbitData = csv.GetRecords<RawNeoOrbitData>();
            Insist.IsNotNull(neoOrbitData, "NEO orbit data cannot be null");

            var neos = new List<CelestialBody>();
            foreach (var body in neoOrbitData)
            {
                var entity = AddEntity(new CelestialBody
                {
                    Type = BodyType.Neo,
                    Name = body.Name.Trim(),
                    ArgumentOfPerihelion = body.Peri,
                    AscendingNode = body.Node,
                    Eccentricity = body.E,
                    Epoch = body.Epoch,
                    Inclination = body.I,
                    MeanAnomaly = body.M,
                    MeanMotion = body.N,
                    SemiMajorAxis = body.A, 
                    Color = System.Drawing.Color.Gray,
                    DiameterKm = body.Diameter ?? 0.5,
                    OsculationDate = mjdBase.AddDays(body.Epoch)
                });

                entity.AddComponent<Orbit>();
                entity.AddComponent<Body>();
                entity.AddComponent<BodyName>();
                
                neos.Add(entity);
            }

            neos.AddRange(planets);
            return neos;
        }

        public override void Update()
        {
            base.Update();

            CheckForEscapePress();

            if (InputBlocked)
            {
            }
            else
            {
                CheckForMouseInput();

            }
        }

        private void CheckForEscapePress()
        {
            if (Input.IsKeyPressed(Keys.Escape))
            {
                InputBlocked = !InputBlocked;
            }
        }

        private void CheckForMouseInput()
        {
            if (Input.LeftMouseButtonPressed)
            {
                var bodies = EntitiesOfType<CelestialBody>();
                var names = bodies.ConvertAll(input => input.GetComponent<BodyName>());
                var sortedList = Utils.Input.SortNamesByDistanceToCamera(ArcCamera, names);
                var mousePos = Input.MousePosition;

                foreach (var name in sortedList)
                {
                    var clicked = name.CheckIfClicked(mousePos);

                    if (clicked && name.Entity.Enabled)
                    {
                        SelectedBodyManager.SetSelected(name.body);
                        
                        break;
                    }
                }
            }
        }

        public void EnableEarthOnlyView(Body bodyToFocus)
        {
            EarthOnlyView = true;
            
            Entities.EntitiesOfType<CelestialBody>().ForEach(entity =>
            {
                if (entity.Name == "Sun" || entity.Name == "Earth" || entity.GetComponent<Body>().Equals(bodyToFocus))
                {
                    entity.Enabled = true;
                }
                else
                {
                    entity.Enabled = false;
                }
            });
        }

        public void DisableEarthOnlyView()
        {
            EarthOnlyView = false;
            
            Entities.EntitiesOfType<CelestialBody>().ForEach(entity => entity.Enabled = true);
        }
    }
}