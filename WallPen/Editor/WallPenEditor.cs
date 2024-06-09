using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using System.ComponentModel;

namespace WallPen.Editor
{
    [InitializeOnLoad]
    static class WallPenInitializer
    {
        static WallPenInitializer()
        {
            if (WallPenEditor.instance == null)
                ScriptableObject.CreateInstance<WallPenEditor>().Init();
        }
    }
    [System.Serializable]
    public class WallPenEditor : EditorWindow
    {
        public static WallPenEditor instance;

        WallSet internalWalls;

        public Material internalFloorMat;
        public Material floorMaterial
        {
            get { return internalFloorMat; }
            set
            {
                EditorPrefs.SetString(WallPenEditorKeys.floorMaterial, AssetDatabase.GetAssetPath(value));
                internalFloorMat = value;
            }
        }
        public WallSet walls
        {
            get { return internalWalls; }
            set
            {
                string path = AssetDatabase.GetAssetPath(value);
                EditorPrefs.SetString(WallPenEditorKeys.currentWallset, AssetDatabase.GetAssetPath(value));
                internalWalls = value;
            }
        }

        WallpenInterior interior;
        public Transform wallParent;
        public Transform floorParent;


        //Drawing floors
        bool canDraw;
        bool isDragging;
        Vector3 floorPoint1;
        Vector3 floorPoint2;
        FloorPiece piece;

        //Floorpiece mesh
        GameObject floorpieceViz;

        bool internalFloorFaceDirection;
        bool displayedNoIconWarning;
        public bool floorFaceDirection
        {
            get { return internalFloorFaceDirection; }
            set
            {
                EditorPrefs.SetBool(WallPenEditorKeys.floorDirection, value);
                internalFloorFaceDirection = value;
            }
        }
        Texture2D drawIcon;
        Texture2D floorIcon;
        Texture2D drawOptions;

        public int wallUndoOperation;

        public enum BuildMode
        {
            Walls,
            Floors
        }

        BuildMode internalMode;
        public BuildMode mode
        {
            get { return internalMode; }
            set
            {
                EditorPrefs.SetInt(WallPenEditorKeys.currentMode, (int)value);
                internalMode = value;
            }
        }

        public bool optionsWindowOpen;

        public enum WallMode
        {
            Create,
            Destroy
        }
        public WallMode wallMode;

        public ToolbarPreferences toolbarPreferences;

        #region Initialization/Destroy Code
        public void Init()
        {
            if (instance == null)
            {
                Initialize();
            }
            else
                instance.Initialize();
        }

        public void Initialize()
        {
            instance = this;

            LoadPreferences();
            #if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
            #else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
            #endif
        }

        public void LoadPreferences()
        {
            mode = (BuildMode)EditorPrefs.GetInt(WallPenEditorKeys.currentMode);
            floorFaceDirection = EditorPrefs.GetBool(WallPenEditorKeys.floorDirection);
            internalFloorMat = (Material)AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString(WallPenEditorKeys.floorMaterial), typeof(Material));
            walls = (WallSet)AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString(WallPenEditorKeys.currentWallset), typeof(WallSet));
        }

        public void Destroy()
        {
            instance = null;
        }



        #endregion


        #region Fun Editor Window Code

        void OnSceneGUI(SceneView sceneView)
        {
            #region Changing wall modes
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftControl)
            {
                canDraw = true;
                wallMode = WallMode.Destroy;
            }
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftControl)
            {
                canDraw = false;
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftShift)
            {
                canDraw = true;
                wallMode = WallMode.Create;
            }
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftShift)
            {
                canDraw = false;
            }
            #endregion
            if (!isDragging && Selection.activeGameObject)
            {
                if (!Selection.activeGameObject.TryGetComponent(out interior))
                {
                    if (Selection.activeGameObject.transform.parent)
                        Selection.activeGameObject.transform.parent.TryGetComponent(out interior);
                }
            }

            DrawGrid();


            #region Drawing walls and floors and stuff
            if (interior)
            {
                Plane mousePlane = new Plane(Vector3.up, -interior.transform.position.y);
                Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                if (mousePlane.Raycast(r, out float dist))
                {
                    Vector3 point = r.GetPoint(dist);
                    Vector3 roundedPoint = new Vector3(Mathf.Round(point.x), interior.transform.position.y, Mathf.Round(point.z));
                    if (canDraw)
                        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    if (mode == BuildMode.Floors)
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            if (canDraw)
                            {
                                isDragging = true;
                                floorParent = interior.transform.Find("Floors");
                                if (!floorParent)
                                {
                                    floorParent = new GameObject("Floors").transform;
                                    floorParent.transform.parent = interior.transform;
                                    floorParent.transform.localPosition = Vector3.zero;
                                    floorParent.transform.localRotation = Quaternion.identity;
                                }
                                piece = new FloorPiece();
                                floorPoint1 = roundedPoint;
                                floorpieceViz = GameObject.CreatePrimitive(PrimitiveType.Quad);
                                floorpieceViz.transform.forward = Vector3.down;
                                floorpieceViz.transform.parent = floorParent;
                                if (!floorFaceDirection)
                                    floorpieceViz.transform.forward = Vector3.up;

                                if(internalFloorMat)
                                {
                                    Renderer rend = floorpieceViz.GetComponent<Renderer>();
                                    rend.material = internalFloorMat;
                                    rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                                }
                                floorpieceViz.gameObject.name = "Floor Piece";

                            }
                        }
                        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
                        {
                            if (isDragging)
                            {
                                floorPoint2 = roundedPoint;
                                piece.min = Vector3.Min(floorPoint1, floorPoint2);
                                piece.max = Vector3.Max(floorPoint1, floorPoint2);
                                floorpieceViz.transform.position = Vector3.Lerp(piece.min, piece.max, 0.5f);
                                Vector3 scale = new Vector3(piece.max.x - piece.min.x, piece.max.z - piece.min.z, 1);
                                floorpieceViz.transform.localScale = scale;
                            }
                        }

                        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                        {
                            if (isDragging)
                            {
                                isDragging = false;
                                if (piece.min == piece.max)
                                    DestroyImmediate(floorpieceViz);
                                else
                                    Undo.RegisterCreatedObjectUndo(floorpieceViz, "WallPen Floor");
                            }
                        }


                        Handles.color = Color.yellow;
                        Vector3 gizmoPoint = new Vector3(roundedPoint.x, interior.transform.position.y, roundedPoint.z);

                        Vector3[] verts = new Vector3[]
                        {
                            gizmoPoint + new Vector3(-0.5f, 0, -0.5f),
                            gizmoPoint + new Vector3(-0.5f, 0, 0.5f),
                            gizmoPoint + new Vector3(0.5f, 0, 0.5f),
                            gizmoPoint + new Vector3(0.5f, 0, -0.5f)
                        };

                        Handles.DrawSolidRectangleWithOutline(verts, Color.yellow, Color.black);
                    }
                    //PLACING WALLS
                    if (mode == BuildMode.Walls)
                    {
                        //PRESSING MOUSE DOWN
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if (canDraw)
                            {
                                if (!interior)
                                {
                                    Debug.LogError("You need to have a object with a Interior component selected to draw on one!");
                                    return;
                                }
                                else if (!walls)
                                {
                                    Debug.LogError("You need to select a WallSet before you draw walls!");
                                    return;
                                }
                                isDragging = true;

                                //Find/Create the wall parent
                                wallParent = interior.transform.Find("Walls");
                                if (!wallParent)
                                {
                                    wallParent = new GameObject("Walls").transform;
                                    wallParent.transform.parent = interior.transform;
                                    wallParent.transform.localPosition = Vector3.zero;
                                    wallParent.transform.localRotation = Quaternion.identity;
                                }

                                Undo.RegisterCompleteObjectUndo(interior, "Created Wall");
                                wallUndoOperation = Undo.GetCurrentGroup();
                            }
                        }

                        //DRAGGING
                        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
                        {
                            if (isDragging)
                            {
                                Vector2Int mapPoint = interior.WorldToInterior(roundedPoint);
                                TileCell.CellType oldType = interior.cells[interior.coordinateToIndex(mapPoint.x, mapPoint.y)].type;
                                if (wallMode == WallMode.Create)
                                {
                                    interior.cells[interior.coordinateToIndex(mapPoint.x, mapPoint.y)].type = TileCell.CellType.Wall;
                                }
                                if (wallMode == WallMode.Destroy)
                                    interior.cells[interior.coordinateToIndex(mapPoint.x, mapPoint.y)].type = TileCell.CellType.Empty;

                                UpdateCell(mapPoint, oldType);
                            }
                        }

                        //LETTING GO
                        if (Event.current.type == EventType.MouseUp)
                        {
                            if (isDragging)
                            {
                                isDragging = false;
                                EditorUtility.SetDirty(interior);
                            }
                        }

                        #region Drawing the rectangle under the mouse
                        Color fill = Color.HSVToRGB(0.6f, 0.8f, 1);
                        Handles.color = Color.white;
                        Vector3 gizmoPoint = new Vector3(roundedPoint.x, interior.transform.position.y, roundedPoint.z);

                        Vector3[] verts = new Vector3[]
                        {
                            gizmoPoint + new Vector3(-0.5f, 0, -0.5f),
                            gizmoPoint + new Vector3(-0.5f, 0, 0.5f),
                            gizmoPoint + new Vector3(0.5f, 0, 0.5f),
                            gizmoPoint + new Vector3(0.5f, 0, -0.5f)
                        };
                        Handles.DrawSolidRectangleWithOutline(verts, fill, Color.black);
                        #endregion
                    }
                }
            }

            #endregion

            LoadIcons();
            Handles.BeginGUI();
            {
                Texture2D icon = drawIcon;
                string tip = "Draw walls";
                string backupText = "WALL"; //Text used in place of icons incase the icons cant load
                if (mode == BuildMode.Floors)
                {
                    icon = floorIcon;
                    tip = "Draw floors";
                    backupText = "FLOOR";
                }


                float menuHorizPos = 5;
                float optionsButtonWidth = 17;

                float modeButtonWidth = 38;
                if (drawIcon == null)
                    modeButtonWidth = 60;
                float wallsetButtonWidth = 150;
                float totalToolbarWidth = optionsButtonWidth + modeButtonWidth + wallsetButtonWidth;
                float lowestPartOfToolbar = 5 + 32;
                //Options button
                if (GUI.Button(new Rect(menuHorizPos, 5, optionsButtonWidth, 32), new GUIContent(drawOptions)))
                {
                    optionsWindowOpen = !optionsWindowOpen;

                    //Creates a new editor window and shows it as a drop down
                    toolbarPreferences = ScriptableObject.CreateInstance<ToolbarPreferences>();
                    toolbarPreferences.editor = this;
                    Rect screenRect = SceneView.lastActiveSceneView.position;
                    float menuHeight = 40;
                    toolbarPreferences.ShowAsDropDown(new Rect(screenRect.x + 5, screenRect.y + menuHeight + lowestPartOfToolbar, 0, 0), new Vector2(totalToolbarWidth, 140));
                }
                GUIContent content;
                if (drawIcon == null)
                    content = new GUIContent(backupText, tip);
                else
                    content = new GUIContent(icon, tip);
                //Wall/Floor button
                if (GUI.Button(new Rect(menuHorizPos + optionsButtonWidth, 5, modeButtonWidth, 32), content))
                {
                    if (mode == BuildMode.Floors)
                        mode = BuildMode.Walls;
                    else
                        mode = BuildMode.Floors;
                }

                walls = (WallSet)EditorGUI.ObjectField(new Rect(menuHorizPos + optionsButtonWidth + modeButtonWidth, 5, wallsetButtonWidth, 32), "", walls, typeof(WallSet), false);
            }
            Handles.EndGUI();
        }
        #endregion

        #region All the stuff to do with cells and stuff

        public void UpdateCell(Vector2Int point, TileCell.CellType oldType)
        {
            TileCell cell = interior.cells[interior.coordinateToIndex(point.x, point.y)];
            DetailedNeighbours neighbours = GetNeighbours(point.x, point.y);
            //If erased
            if (cell.type == TileCell.CellType.Empty)
            {
                if (cell.prefab) //Destroy the prefab in that cell
                {
                    Undo.DestroyObjectImmediate(cell.prefab);
                    Undo.CollapseUndoOperations(wallUndoOperation);
                }

                foreach (Vector2Int i in neighbours.directions)
                {
                    TileCell neighbour = interior.cells[interior.coordinateToIndex(point.x + i.x, point.y + i.y)];
                    if (neighbour.prefab)
                    {
                        Undo.DestroyObjectImmediate(neighbour.prefab);
                        Undo.CollapseUndoOperations(wallUndoOperation);
                    }

                    PlaceJunctionPrefab(neighbour, oldType);
                }
            }
            else
            {
                if(cell.prefab)
                {

                }
                //If what we placed is at the end of a line
                if (IsCellJunction(point.x, point.y))
                {
                    PlaceJunctionPrefab(cell, oldType);
                }
                else //If placed to fill in a gap
                {
                    GameObject wall = Instantiate(walls.wall, wallParent);
                    wall.transform.position = VectorUtils.GridToWorld(point) + (Vector3.up * interior.transform.position.y);
                    if (neighbours.hasUpDir && neighbours.hasDownDir)
                    {
                        wall.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }

                    if (neighbours.hasLeftDir && neighbours.hasRightDir)
                    {
                        wall.transform.localRotation = Quaternion.identity;
                    }
                    cell.prefab = wall;

                    Undo.RegisterCreatedObjectUndo(wall, "created wall");
                    Undo.CollapseUndoOperations(wallUndoOperation);
                }

                //Correcting neighbour prefabs
                foreach (Vector2Int i in neighbours.directions)
                {
                    TileCell neighbour = interior.cells[interior.coordinateToIndex(point.x + i.x, point.y + i.y)];

                    if (neighbour.prefab)
                    {
                        if (IsCellJunction(point.x + i.x, point.y + i.y))
                        {
                            Undo.DestroyObjectImmediate(neighbour.prefab);
                            Undo.CollapseUndoOperations(wallUndoOperation);
                            PlaceJunctionPrefab(neighbour, oldType);
                        }
                        else
                        {
                            Undo.DestroyObjectImmediate(neighbour.prefab);
                            Undo.CollapseUndoOperations(wallUndoOperation);

                            DetailedNeighbours secondNeighbours = GetNeighbours(neighbour.position.x, neighbour.position.y);
                            GameObject wall = Instantiate(walls.wall, wallParent);
                            wall.transform.position = VectorUtils.GridToWorld(neighbour.position) + (Vector3.up * interior.transform.position.y);
                            if (secondNeighbours.hasUpDir && secondNeighbours.hasDownDir)
                            {
                                wall.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                            }

                            if (secondNeighbours.hasLeftDir && secondNeighbours.hasRightDir)
                            {
                                wall.transform.localRotation = Quaternion.identity;
                            }
                            neighbour.prefab = wall;

                            Undo.RegisterCreatedObjectUndo(wall, "Wall Creation");
                            Undo.CollapseUndoOperations(wallUndoOperation);
                        }
                    }
                    else
                        PlaceJunctionPrefab(neighbour, oldType);
                }
            }

            Undo.CollapseUndoOperations(wallUndoOperation);
        }

        public TileCell GetJunctionInDirection(TileCell cell, Vector2Int dir)
        {
            if (dir == Vector2Int.up)
            {
                return GetNextUpTile(cell.position.x, cell.position.y);
            }
            if (dir == Vector2Int.down)
            {
                return GetNextDownTile(cell.position.x, cell.position.y);
            }
            if (dir == Vector2Int.left)
            {
                return GetNextLeftTile(cell.position.x, cell.position.y);
            }
            if (dir == Vector2Int.right)
            {
                return GetNextRightTile(cell.position.x, cell.position.y);
            }

            return null;
        }

        #region Wall Creators
        public void CreateUpWall(TileCell cell, GameObject piece, bool placeCaps)
        {
            TileCell up = GetNextUpTile(cell.position.x, cell.position.y);

            //If there's already a wall recorded for these two cells
            if (interior.Walls.Exists(x => (x.one == cell || x.two == cell) && (x.one == up || x.two == up)))
                return;
            //If they are right next to each other, just don't do anything
            if (Vector2Int.Distance(cell.position, up.position) == 1)
                return;

            Vector2Int currentPos = cell.position;
            Vector2Int upPos = up.position;

            if (placeCaps)
            {
                currentPos += Vector2Int.up;
                upPos -= Vector2Int.up;
            }

            //INSTANTIATING
            GameObject upWall = Instantiate(piece, wallParent);

            upWall.transform.position = VectorUtils.GridToWorld(VectorUtils.Center(currentPos, upPos));
            upWall.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));

            Vector3 upScale = new Vector3((DistanceBetweenWorldSpaceCoords(currentPos, upPos)) - 1f, 1, 1);
            upWall.transform.localScale = upScale;

            interior.Walls.Add(new Wall(cell, up, upWall));
        }

        public void CreateRightWall(TileCell cell, GameObject piece, bool placeCaps)
        {
            TileCell right = GetNextRightTile(cell.position.x, cell.position.y);

            //If there's already a wall recorded for these two cells
            if (interior.Walls.Exists(x => (x.one == cell || x.two == cell) && (x.one == right || x.two == right)))
                return;
            //If they are right next to each other, just don't do anything
            if (Vector2Int.Distance(cell.position, right.position) == 1)
                return;

            Vector2Int currentPos = cell.position;
            Vector2Int rightPos = right.position;

            if (placeCaps)
            {
                currentPos += Vector2Int.right;
                rightPos -= Vector2Int.right;
            }

            GameObject rightWall = Instantiate(piece, wallParent);

            rightWall.transform.position = VectorUtils.GridToWorld(VectorUtils.Center(currentPos, rightPos));
            rightWall.transform.localRotation = Quaternion.identity;

            Vector3 rightScale = new Vector3(DistanceBetweenWorldSpaceCoords(currentPos, rightPos) - 1f, 1, 1);
            rightWall.transform.localScale = rightScale;

            interior.Walls.Add(new Wall(cell, right, rightWall));
        }

        #endregion

        public DetailedNeighbours GetNeighbours(int xCoord, int yCoord)
        {
            DetailedNeighbours neighbours = new DetailedNeighbours();

            neighbours.directions = new List<Vector2Int>();
            //Check top
            if (interior.coordinateInRange(xCoord, yCoord + 1))
            {
                if (interior.cells[interior.coordinateToIndex(xCoord, yCoord + 1)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasUpDir = true;
                    neighbours.directions.Add(Vector2Int.up);
                }

            }
            //Check Bottom
            if (interior.coordinateInRange(xCoord, yCoord - 1))
            {
                if (interior.cells[interior.coordinateToIndex(xCoord, yCoord - 1)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasDownDir = true;
                    neighbours.directions.Add(Vector2Int.down);
                }

            }
            //Check left
            if (interior.coordinateInRange(xCoord - 1, yCoord))
            {
                if (interior.cells[interior.coordinateToIndex(xCoord - 1, yCoord)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasRightDir = true;
                    neighbours.directions.Add(Vector2Int.left);
                }

            }
            //Check right
            if (interior.coordinateInRange(xCoord + 1, yCoord))
            {
                if (interior.cells[interior.coordinateToIndex(xCoord + 1, yCoord)].type == TileCell.CellType.Wall)
                {
                    neighbours.hasLeftDir = true;
                    neighbours.directions.Add(Vector2Int.right);
                }
            }

            return neighbours;
        }

        #region Tile Getters
        public TileCell GetNextRightTile(int x, int y)
        {
            for (int current = x + 1; current < interior.MaxInteriorSize; current++)
            {
                if (interior.cells[interior.coordinateToIndex(current, y)] != null)
                    if (IsCellJunction(current, y))
                        return interior.cells[interior.coordinateToIndex(current, y)];
            }
            return null;
        }

        public TileCell GetNextUpTile(int x, int y)
        {
            for (int current = y + 1; current < interior.MaxInteriorSize; current++)
            {
                if (interior.cells[interior.coordinateToIndex(x, current)] != null)
                    if (IsCellJunction(x, current))
                        return interior.cells[interior.coordinateToIndex(x, current)];
            }


            return null;
        }

        public TileCell GetNextLeftTile(int x, int y)
        {
            for (int current = x - 1; current > 0; current--)
            {
                if (interior.cells[interior.coordinateToIndex(current, y)] != null)
                    if (IsCellJunction(current, y))
                        return interior.cells[interior.coordinateToIndex(current, y)];
            }
            return null;
        }

        public TileCell GetNextDownTile(int x, int y)
        {
            for (int current = y - 1; current > 0; current--)
            {
                if (interior.cells[interior.coordinateToIndex(x, current)] != null)
                    if (IsCellJunction(x, current))
                        return interior.cells[interior.coordinateToIndex(x, current)];
            }


            return null;
        }
        #endregion

        public void PlaceJunctionPrefab(TileCell cell, TileCell.CellType oldCellType, bool updatingNeighbour = false)
        {
            if (updatingNeighbour)
            {
                if (cell.type != TileCell.CellType.Wall)
                {
                    cell.type = oldCellType;
                    return;
                }

            }
            if (cell.prefab != null) //If there's already a prefab in this cell, return.
                return;

            GameObject obj = null;
            DetailedNeighbours neighbours = GetNeighbours(cell.position.x, cell.position.y);
            if (neighbours.directions.Count == 0)
            {
                if(walls.pole)
                {
                    obj = Instantiate(walls.pole, wallParent);
                    obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                    cell.prefab = obj;
                }
                else
                {
                    Debug.LogError("Wallset '" + walls.name + "' doesn't have a pole prefab!");
                    cell.type = oldCellType;
                    return;
                }
            }
            if (neighbours.directions.Count == 1)
            {
                if(walls.stub)
                {
                    obj = Instantiate(walls.stub, wallParent);
                    obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                    obj.transform.forward = VectorUtils.GridToWorld(-neighbours.directions[0]);
                    cell.prefab = obj;
                }
                else
                {
                    Debug.LogError("Wallset '" + walls.name + "' doesn't have a stub prefab!");
                    cell.type = oldCellType;
                    return;
                }
            }

            if (neighbours.directions.Count == 2)
            {
                Vector3 direction = VectorUtils.GridToWorld(VectorUtils.AverageDirection(neighbours.directions.ToArray()));
                if (direction != Vector3.zero)
                {
                    if(walls.corner)
                    {
                        obj = Instantiate(walls.corner, wallParent);
                        obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                        Vector3 dir = Quaternion.LookRotation(direction).eulerAngles;
                        dir.y += 45;
                        obj.transform.rotation = Quaternion.Euler(dir);
                        cell.prefab = obj;
                    }
                    else
                    {
                        Debug.LogError("Wallset '" + walls.name + "' doesn't have a corner prefab!");
                        cell.type = oldCellType;
                        return;
                    }
                }
                else
                {
                    if(walls.wall)
                    {
                        obj = Instantiate(walls.wall, wallParent);
                        obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                        obj.transform.forward = Vector3.right;
                        cell.prefab = obj;
                    }
                    else
                    {
                        Debug.LogError("Wallset '" + walls.name + "' doesn't have a wall prefab!");
                        cell.type = oldCellType;
                        return;
                    }
                }
            }

            if (neighbours.directions.Count == 3)
            {
                if(walls.TPoint)
                {
                    obj = Instantiate(walls.TPoint, wallParent);
                    obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                    obj.transform.forward = VectorUtils.GridToWorld(VectorUtils.AverageDirection(neighbours.directions.ToArray()));
                    cell.prefab = obj;
                }
                else
                {
                    Debug.LogError("Wallset '" + walls.name + "' doesn't have a t-point prefab!");
                    cell.type = oldCellType;
                    return;
                }
            }

            if (neighbours.directions.Count == 3)
            {
                if (walls.cross)
                {
                    obj = Instantiate(walls.cross, wallParent);
                    obj.transform.position = VectorUtils.GridToWorld(cell.position) + (Vector3.up * interior.transform.position.y);
                    obj.transform.forward = VectorUtils.GridToWorld(VectorUtils.AverageDirection(neighbours.directions.ToArray()));
                    cell.prefab = obj;
                }
                else
                {
                    Debug.LogError("Wallset '" + walls.name + "' doesn't have a cross prefab!");
                    cell.type = oldCellType;
                    return;
                }
            }
            if (obj != null)
            {
                Undo.RegisterCreatedObjectUndo(obj, "created wall");
                Undo.CollapseUndoOperations(wallUndoOperation);
            }
            Debug.Log("Created Wall");
            Undo.CollapseUndoOperations(wallUndoOperation);
        }
        #endregion


        public void DrawGrid()
        {
            if (interior != null)
            {
                for (int x = 0; x < interior.MaxInteriorSize; x++)
                {
                    for (int y = 0; y < interior.MaxInteriorSize; y++)
                    {
                        Vector3 add = new Vector3(x, 0, y) - new Vector3(0.5f, 0f, 0.5f);
                        add.y = interior.transform.position.y;
                        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        Handles.DrawAAPolyLine(new Vector3[] { Vector3.zero + add, Vector3.right + add, (Vector3.right + Vector3.forward) + add, Vector3.forward + add, Vector3.zero + add });
                    }
                }
            }
        }

        public void LoadIcons()
        {
            drawIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/WallPen/Icons/walls.png", typeof(Texture2D));
            floorIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/WallPen/Icons/floors.png", typeof(Texture2D));
            drawOptions = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/WallPen/Icons/drawOptions.png", typeof(Texture2D));

            if ((drawIcon == null || floorIcon == null || drawOptions == null) && !displayedNoIconWarning)
            {
                displayedNoIconWarning = true;
                Debug.LogWarning("WallPen couldn't get the icons for the toolbar! Make sure WallPen is in the Assets folder!");
            }
        }

        #region Utility Methods

        public float DistanceBetweenWorldSpaceCoords(Vector2Int one, Vector2Int two, float multiplier = 1)
        {
            return Vector3.Distance((VectorUtils.GridToWorld(one) * multiplier), (VectorUtils.GridToWorld(two) * multiplier));
        }

        /// <summary>
        /// Checks if the current cell is a corner, T-Corner, or stub. Anything that isn't just a line basically. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsCellJunction(int x, int y, List<TileCell.CellType> extraTypes = null)
        {
            DetailedNeighbours neighbours = GetNeighbours(x, y);
            Vector3 direction = VectorUtils.GridToWorld(VectorUtils.AverageDirection(neighbours.directions.ToArray()));
            bool isCorner = direction != Vector3.zero;
            return neighbours.directions.Count == 1 || (neighbours.directions.Count == 2 && isCorner) || neighbours.directions.Count == 4 || neighbours.directions.Count == 8 || neighbours.directions.Count == 3 || neighbours.directions.Count == 6 || neighbours.directions.Count == 12 || neighbours.directions.Count == 9 || neighbours.directions.Count == 11 || neighbours.directions.Count == 7 || neighbours.directions.Count == 14 || neighbours.directions.Count == 13 || neighbours.directions.Count == 0;
        }
        #endregion



        public static Texture2D LoadIcon(string iconName)
        {
            var img = LoadInternalAsset<Texture2D>(iconName);
            return img;
        }

        public static T LoadInternalAsset<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

    }



    //Extra Classes and shit
    #region Classes or whatever

    #region Level Creation

    public class DetailedNeighbours
    {
        public bool hasUpDir;
        public bool hasDownDir;
        public bool hasRightDir;
        public bool hasLeftDir;

        public List<Vector2Int> directions;
    }
    #endregion

    public class FloorPiece
    {
        public Vector3 min;
        public Vector3 max;
    }
    #endregion
}
