using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Running;
    public List<Node> nodes = new List<Node>();
    public Blackboard blackboard = new Blackboard();

    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;

#if UNITY_EDITOR
        node.guid = GUID.Generate().ToString();
        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
#endif

        nodes.Add(node);

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");

        AssetDatabase.SaveAssets();
#endif
        return node;
    }

    public void DeleteNode(Node node)
    {

#if UNITY_EDITOR
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
#endif
        nodes.Remove(node);

        // AssetDatabase.RemoveObjectFromAsset(node);
#if UNITY_EDITOR
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
#endif
    }

    public void AddChild(Node parent, Node child)
    {
        RootNode root = parent as RootNode;
        if (root)
        {
#if UNITY_EDITOR
            Undo.RecordObject(root, "Behaviour Tree (AddChild)");
#endif

            root.child = child;

#if UNITY_EDITOR
            EditorUtility.SetDirty(root);
#endif
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
#if UNITY_EDITOR
            Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
#endif
            decorator.child = child;
#if UNITY_EDITOR
            EditorUtility.SetDirty(decorator);
#endif
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
#if UNITY_EDITOR
            Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
#endif
            composite.children.Add(child);
#if UNITY_EDITOR
            EditorUtility.SetDirty(composite);
#endif
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        RootNode root = parent as RootNode;
        if (root)
        {
#if UNITY_EDITOR
            Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
#endif
            root.child = null;
#if UNITY_EDITOR
            EditorUtility.SetDirty(root);
#endif
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
#if UNITY_EDITOR
            Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
#endif
            decorator.child = null;
#if UNITY_EDITOR
            EditorUtility.SetDirty(decorator);
#endif
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
#if UNITY_EDITOR
            Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
#endif
            composite.children.Remove(child);
#if UNITY_EDITOR
            EditorUtility.SetDirty(composite);
#endif
        }
    }

    public static List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }

        return children;
    }

    public static void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach(n => Traverse(n, visiter));
        }
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        tree.nodes = new List<Node>();
        Traverse(tree.rootNode, (n) =>
        {
            tree.nodes.Add(n);
        });
        return tree;
    }

    public void Bind(Context context)
    {
        Traverse(rootNode, node =>
        {
            node.context = context;
            node.blackboard = blackboard;
        });
    }
}
