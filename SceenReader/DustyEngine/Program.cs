using System;
using System.Collections.Generic;
using System.IO;

namespace DustyEngine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string SceenName = Console.ReadLine();

            if (SceenName == "DefaultSceen")
            {
                PlaySceen("C:\\Users\\mini6\\Desktop\\DustyEngine\\DustyEngine\\DefaultSceen.txt");
            }
            else
            {
                try
                {
                    PlaySceen(SceenName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        private static void PlaySceen(string SceenName)
        {
            Sceen sceen = new Sceen(SceenName);

            if (sceen.GameObjects != null)
            {
                Console.WriteLine("Sceen:");

                foreach (GameObject gameObject in sceen.GameObjects)
                {
                    Console.WriteLine("  " + gameObject.Name);
                    int i = 0;
                    foreach (Component component in gameObject.Components)
                    {
                        i++;
                        if (gameObject.Components == null)
                        {
                            Console.WriteLine("     None components");
                        }
                        else
                        {
                            Console.WriteLine("    " + component.Name);
                        }
                    }

                    //    Console.WriteLine("Com:" + i);
                    Console.WriteLine("");
                }

                Console.ReadLine();
            }
        }
    }


    class Sceen
    {
        public GameObject[] GameObjects;

        public Sceen(string sceen)
        {
            GameObjects = ReadGameObjectsFromFile(sceen);
        }

        private GameObject[] ReadGameObjectsFromFile(string filePath)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                GameObject currentObject = null;

                foreach (var line in lines)
                {
                    if (line.StartsWith("GameObject"))
                    {
                        if (currentObject != null)
                        {
                            gameObjects.Add(currentObject);
                        }

                        currentObject = new GameObject();
                        currentObject.Name = line.Substring("GameObject_".Length).Trim();
                    }
                    else if (currentObject != null)
                    {
                        string componentName = line.Trim();
                        if (!string.IsNullOrEmpty(componentName))
                        {
                            currentObject.AddComponent(new Component(componentName));
                        }
                    }
                }

                if (currentObject != null)
                {
                    gameObjects.Add(currentObject);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return gameObjects.ToArray();
        }
    }

    class GameObject
    {
        public string Name;
        public List<Component> Components = new List<Component>();

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }
    }

    class Component
    {
        public string Name;

        public Component(string name)
        {
            Name = name;
        }
    }
}