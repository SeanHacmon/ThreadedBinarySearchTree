using System;
using System.Threading;

namespace ThreadedBinarySearchTree
{
    class Node
    {
        public Node left = null;
        public Node right = null;
        public int data = int.MinValue;       
    }


    public class ThreadedBinarySearchTree
    {
        // init
        private ReaderWriterLockSlim nodeLock = new ReaderWriterLockSlim();
        private Node root{ get; set;}

        // constructor
        public ThreadedBinarySearchTree(){}

        // adding a node after searching it's not in the tree
        public void add(int num)
        {
            if (!search(num))
            {
                try
                {
                    // Requesting to enter critical section.
                    nodeLock.EnterWriteLock();
                    Node curr = this.root;
                    if (this.root == null)
                    {
                        this.root = new Node();
                        this.root.data = num;
                    }
                    else
                    {
                        while (curr != null)
                        {
                            if (curr.data < num)
                            {
                                if (curr.right != null)
                                    curr = curr.right;
                                else
                                {
                                    curr.right = new Node();
                                    curr.right.data = num;
                                    return;
                                }
                            }
                            else // curr.data > num
                            {
                                if (curr.left != null)
                                    curr = curr.left;
                                else
                                {
                                    curr.left = new Node();
                                    curr.left.data = num;
                                    return;
                                }

                            }

                        }
                    }
                }
                finally
                {
                    // Release the Lock.
                    nodeLock.ExitWriteLock();
                }
            }

        }


        public void remove(int num)
        {
            if (search(num))
            {
                try
                {
                    nodeLock.EnterWriteLock();
                    Node curr = this.root;
                    this.root = deleteRec(root, num);
                }
                finally
                {
                    // Release the Lock.
                    nodeLock.ExitWriteLock();
                }
            }
        }

        public bool search(int num)
        {
            // Entering critical section.
            nodeLock.EnterReadLock();
            try
            {
                Node curr = this.root;
                while (curr != null)
                {
                    if (curr.data == num)
                        return true;
                    if (curr.data < num)
                        curr = curr.left;
                    else if (curr.data > num)
                        curr = curr.right;
                }
                    return false;
            }
            finally
            {
                // Release the Lock.
                nodeLock.ExitReadLock();
            }
     
        }

        public void clear()
        {
            while (this.root != null)
            {
                remove(this.root.data);
            }
        }

        public void print()
        {
            nodeLock.EnterReadLock();
            try
            {
                inorderRec(this.root);
            }
            finally
            {
                nodeLock.ExitReadLock();
            }
        }

        // Helper Functions.

        private Node deleteRec(Node root, int key)
        {
            /* Base Case: If the tree is empty */
            if (root == null)
                return root;

            /* Otherwise, recur down the tree */
            if (key < root.data)
                root.left = deleteRec(root.left, key);
            else if (key > root.data)
                root.right = deleteRec(root.right, key);

            // if key is same as root's
            // key, then This is the
            // node to be deleted
            else
            {
                // node with only one child or no child
                if (root.left == null)
                    return root.right;
                else if (root.right == null)
                    return root.left;

                // node with two children: Get the inorder
                // successor (smallest in the right subtree)
                root.data = minValue(root.right);

                // Delete the inorder successor
                root.right = deleteRec(root.right, root.data);
            }

            return root;
        }

        private int minValue(Node root)
        {
            int minv = root.data;
            while (root.left != null)
            {
                minv = root.left.data;
                root = root.left;
            }
            return minv;
        }

        private void inorderRec(Node root)
        {
            if (root != null)
            {
                inorderRec(root.left);
                Console.Write(root.data + " ");
                inorderRec(root.right);
            }
        }

        }

}
