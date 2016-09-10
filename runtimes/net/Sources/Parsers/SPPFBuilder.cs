/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
*
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/
using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a structure that helps build a Shared Packed Parse Forest (SPPF)
	/// </summary>
	/// <remarks>
	/// A SPPF is a compact representation of multiple variants of an AST at once.
	/// GLR algorithms originally builds the complete SPPF.
	/// However we only need to build one of the variant, i.e. an AST for the user.
	/// </remarks>
	class SPPFBuilder : SemanticBody
	{
		/// <summary>
		/// The initial size of the reduction handle
		/// </summary>
		private const int INIT_HANDLE_SIZE = 1024;
		/// <summary>
		/// The initial size of the history buffer
		/// </summary>
		private const int INIT_HISTORY_SIZE = 8;
		/// <summary>
		/// The initial size of the history parts' buffers
		/// </summary>
		private const int INIT_HISTORY_PART_SIZE = 64;

		/// <summary>
		/// Represents a generation of GSS edges in the current history
		/// The history is used to quickly find pre-existing matching GSS edges
		/// </summary>
		private class HistoryPart
		{
			/// <summary>
			/// The GSS labels in this part
			/// </summary>
			public int[] data;
			/// <summary>
			/// The index of the represented GSS generation
			/// </summary>
			public int generation;
			/// <summary>
			/// The next available slot in the data
			/// </summary>
			public int next;

			/// <summary>
			/// Initializes a new instance
			/// </summary>
			public HistoryPart()
			{
				generation = 0;
				data = new int[INIT_HISTORY_PART_SIZE];
				next = 0;
			}
		}

		/// <summary>
		/// Represents a factory of history parts
		/// </summary>
		private class HistoryPartFactory : Factory<HistoryPart>
		{
			/// <summary>
			///  Creates a new object
			/// </summary>
			/// <param name="pool">The enclosing pool</param>
			/// <returns>The created object</returns>
			public HistoryPart CreateNew(Pool<HistoryPart> pool)
			{
				return new HistoryPart();
			}
		}

		/// <summary>
		/// The pool of history parts
		/// </summary>
		private readonly Pool<HistoryPart> poolHPs;
		/// <summary>
		/// The history
		/// </summary>
		private HistoryPart[] history;
		/// <summary>
		/// The next available slot for a history part
		/// </summary>
		private int nextHP;
		/// <summary>
		/// The SPPF being built
		/// </summary>
		private SPPF sppf;
		/// <summary>
		/// The adjacency cache for the reduction
		/// </summary>
		private SPPFNodeRef[] cacheChildren;
		/// <summary>
		/// The new available slot in the current cache
		/// </summary>
		private int cacheNext;
		/// <summary>
		/// The reduction handle represented as the indices of the sub-trees in the cache
		/// </summary>
		private int[] handleIndices;
		/// <summary>
		/// The actions for the reduction
		/// </summary>
		private TreeAction[] handleActions;
		/// <summary>
		/// The index of the next available slot in the handle
		/// </summary>
		private int handleNext;
		/// <summary>
		/// The stack of semantic objects for the reduction
		/// </summary>
		private int[] stack;
		/// <summary>
		/// The number of items popped from the stack
		/// </summary>
		private int popCount;
		/// <summary>
		/// The AST being built
		/// </summary>
		private readonly ASTSimpleTree result;

		#region Implementation of SemanticBody

		/// <summary>
		/// Gets the symbol at the i-th index
		/// </summary>
		/// <param name="index">Index of the symbol</param>
		/// <returns>The symbol at the given index</returns>
		public SemanticElement this[int index]
		{
			get
			{
				SPPFNodeRef reference = cacheChildren[handleIndices[index]];
				SPPFNode sppfNode = sppf.GetNode(reference.NodeId);
				TableElemRef label = (sppfNode as SPPFNodeNormal).GetVersion(reference.Version).Label;
				return result.GetSemanticElementForLabel(label);
			}
		}

		/// <summary>
		/// Gets the length of this body
		/// </summary>
		public int Length { get { return handleNext; } }

		#endregion

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		/// <param name="tokens">The token table</param>
		/// <param name="variables">The table of parser variables</param>
		/// <param name="virtuals">The table of parser virtuals</param>
		public SPPFBuilder(TokenRepository tokens, ROList<Symbol> variables, ROList<Symbol> virtuals)
		{
			poolHPs = new Pool<HistoryPart>(new HistoryPartFactory(), INIT_HISTORY_SIZE);
			history = new HistoryPart[INIT_HISTORY_SIZE];
			nextHP = 0;
			sppf = new SPPF();
			cacheChildren = new SPPFNodeRef[INIT_HANDLE_SIZE];
			handleIndices = new int[INIT_HANDLE_SIZE];
			handleActions = new TreeAction[INIT_HANDLE_SIZE];
			stack = new int[INIT_HANDLE_SIZE];
			result = new ASTSimpleTree(tokens, variables, virtuals);
		}

		/// <summary>
		/// Gets the history part for the given GSS generation
		/// </summary>
		/// <param name="generation">The index of a GSS generation</param>
		/// <returns>The corresponding history part, or <c>null</c></returns>
		private HistoryPart GetHistoryPart(int generation)
		{
			for (int i = 0; i != nextHP; i++)
				if (history[i].generation == generation)
					return history[i];
			return null;
		}

		/// <summary>
		/// Clears the current history
		/// </summary>
		public void ClearHistory()
		{
			for (int i = 0; i != nextHP; i++)
				poolHPs.Return(history[i]);
			nextHP = 0;
		}

		/// <summary>
		/// Gets the symbol on the specified GSS edge label
		/// </summary>
		/// <param name="label">The label of a GSS edge</param>
		/// <returns>The symbol on the edge</returns>
		public Symbol GetSymbolOn(int label)
		{
			return result.GetSymbolFor(sppf.GetNode(label).OriginalSymbol);
		}

		/// <summary>
		/// Gets the GSS label already in history for the given GSS generation and symbol
		/// </summary>
		/// <param name="generation">The index of a GSS generation</param>
		/// <param name="symbol">A symbol to look for</param>
		/// <returns>The existing GSS label, or the epsilon label</returns>
		public int GetLabelFor(int generation, TableElemRef symbol)
		{
			HistoryPart hp = GetHistoryPart(generation);
			if (hp == null)
				return SPPF.EPSILON;
			for (int i = 0; i != hp.next; i++)
			{
				if (sppf.GetNode(hp.data[i]).OriginalSymbol == symbol)
					return hp.data[i];
			}
			return SPPF.EPSILON;
		}

		/// <summary>
		/// Creates a single node in the result SPPF an returns it
		/// </summary>
		/// <param name="symbol">The symbol as the node's label</param>
		/// <returns>The created node's index in the SPPF</returns>
		public int GetSingleNode(TableElemRef symbol)
		{
			return sppf.NewNode(symbol);
		}

		/// <summary>
		/// Prepares for the forthcoming reduction operations
		/// </summary>
		/// <param name="first">The first label</param>
		/// <param name="path">The path being reduced</param>
		/// <param name="length">The reduction length</param>
		public void ReductionPrepare(int first, GSSPath path, int length)
		{
			// build the stack
			if (length > 0)
			{
				for (int i = 0; i < length - 1; i++)
					stack[i] = path[length - 2 - i];
				stack[length - 1] = first;
			}
			// initialize the reduction data
			cacheNext = 0;
			handleNext = 0;
			popCount = 0;
		}

		/// <summary>
		/// During a reduction, pops the top symbol from the stack and gives it a tree action
		/// </summary>
		/// <param name="action">The tree action to apply to the symbol</param>
		public void ReductionPop(TreeAction action)
		{
			AddToCache(stack[popCount++], action);
		}

		/// <summary>
		/// Adds the specified GSS label to the reduction cache with the given tree action
		/// </summary>
		/// <param name="gssLabel">The label to add to the cache</param>
		/// <param name="action">The tree action to apply</param>
		private void AddToCache(int gssLabel, TreeAction action)
		{
			if (action == TreeAction.Drop)
				return;
			SPPFNode node = sppf.GetNode(gssLabel);
			if (node.IsReplaceable)
			{
				SPPFNodeReplaceable replaceable = node as SPPFNodeReplaceable;
				// this is replaceable sub-tree
				for (int i = 0; i != replaceable.ChildrenCount; i++)
					AddToCache(sppf.GetNode(replaceable.Children[i].NodeId) as SPPFNodeNormal, replaceable.Actions[i]);
			}
			else
			{
				// this is a simple reference to an existing SPPF node
				AddToCache(node as SPPFNodeNormal, action);
			}
		}

		/// <summary>
		/// Adds the specified SPPF node to the cache
		/// </summary>
		/// <param name="node">The node to add to the cache</param>
		/// <param name="action">The tree action to apply onto the node</param>
		private void AddToCache(SPPFNodeNormal node, TreeAction action)
		{
			SPPFNodeVersion version = node.DefaultVersion;
			while (cacheNext + version.ChildrenCount + 1 >= cacheChildren.Length)
			{
				// the current cache is not big enough, build a bigger one
				Array.Resize(ref cacheChildren, cacheChildren.Length + INIT_HANDLE_SIZE);
			}
			// add the node in the cache
			cacheChildren[cacheNext] = new SPPFNodeRef(node.Identifier, 0);
			// setup the handle to point to the root
			if (handleNext == handleIndices.Length)
			{
				Array.Resize(ref handleIndices, handleIndices.Length + INIT_HANDLE_SIZE);
				Array.Resize(ref handleActions, handleActions.Length + INIT_HANDLE_SIZE);
			}
			handleIndices[handleNext] = cacheNext;
			handleActions[handleNext] = action;
			// copy the children
			if (version.ChildrenCount > 0)
				Array.Copy(version.Children, 0, cacheChildren, cacheNext + 1, version.ChildrenCount);
			handleNext++;
			cacheNext += version.ChildrenCount + 1;
		}

		/// <summary>
		/// During a reduction, inserts a virtual symbol
		/// </summary>
		/// <param name="index">The virtual symbol's index</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddVirtual(int index, TreeAction action)
		{
			if (action == TreeAction.Drop)
				return; // why would you do this?
			int nodeId = sppf.NewNode(new TableElemRef(TableType.Virtual, index));
			if (cacheNext + 1 >= cacheChildren.Length)
			{
				// the current cache is not big enough, build a bigger one
				Array.Resize(ref cacheChildren, cacheChildren.Length + INIT_HANDLE_SIZE);
			}
			// add the node in the cache
			cacheChildren[cacheNext] = new SPPFNodeRef(nodeId, 0);
			// setup the handle to point to the root
			if (handleNext == handleIndices.Length)
			{
				Array.Resize(ref handleIndices, handleIndices.Length + INIT_HANDLE_SIZE);
				Array.Resize(ref handleActions, handleActions.Length + INIT_HANDLE_SIZE);
			}
			handleIndices[handleNext] = cacheNext;
			handleActions[handleNext] = action;
			// copy the children
			handleNext++;
			cacheNext++;
		}

		/// <summary>
		/// During a reduction, inserts the sub-tree of a nullable variable
		/// </summary>
		/// <param name="nullable">The sub-tree of a nullable variable</param>
		/// <param name="action">The tree action applied onto the symbol</param>
		public void ReductionAddNullable(int nullable, TreeAction action)
		{
			AddToCache(nullable, action);
		}

		/// <summary>
		/// Finalizes the reduction operation
		/// </summary>
		/// <param name="generation">The generation to reduce from</param>
		/// <param name="varIndex">The reduced variable index</param>
		/// <param name="replaceable">Whether the sub-tree to build must have a replaceable root or not</param>
		/// <returns>The identifier of the produced SPPF node</returns>
		public int Reduce(int generation, int varIndex, bool replaceable)
		{
			int label = replaceable ? ReduceReplaceable(varIndex) : ReduceNormal(varIndex);
			AddToHistory(generation, label);
			return label;
		}

		/// <summary>
		/// Executes the reduction as a normal reduction
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <returns>The identifier of the produced SPPF node</returns>
		private int ReduceNormal(int varIndex)
		{
			TableElemRef promotedSymbol = new TableElemRef();
			SPPFNodeRef promotedReference = new SPPFNodeRef(SPPF.EPSILON, 0);

			int insertion = 0;
			for (int i = 0; i != handleNext; i++)
			{
				switch (handleActions[i])
				{
				case TreeAction.Promote:
					if (promotedReference.NodeId != SPPF.EPSILON)
					{
						// not the first promotion
						// create a new version for the promoted node
						SPPFNodeNormal oldPromotedNode = sppf.GetNode(promotedReference.NodeId) as SPPFNodeNormal;
						SPPFNodeRef oldPromotedRef = oldPromotedNode.NewVersion(promotedSymbol, cacheChildren, insertion);
						// register the previously promoted reference into the cache
						cacheChildren[0] = oldPromotedRef;
						insertion = 1;
					}
					// save the new promoted node
					promotedReference = cacheChildren[handleIndices[i]];
					SPPFNodeNormal promotedNode = sppf.GetNode(promotedReference.NodeId) as SPPFNodeNormal;
					SPPFNodeVersion promotedVersion = promotedNode.GetVersion(promotedReference.Version);
					promotedSymbol = promotedVersion.Label;
					// repack the children on the left if any
					Array.Copy(cacheChildren, handleIndices[i] + 1, cacheChildren, insertion, promotedVersion.ChildrenCount);
					insertion += promotedVersion.ChildrenCount;
					break;
				default:
					// Repack the sub-root on the left
					if (insertion != handleIndices[i])
						cacheChildren[insertion] = cacheChildren[handleIndices[i]];
					insertion++;
					break;
				}
			}

			TableElemRef originalLabel = new TableElemRef(TableType.Variable, varIndex);
			TableElemRef currentLabel = promotedReference.NodeId != SPPF.EPSILON ? promotedSymbol : originalLabel;
			return sppf.NewNode(originalLabel, currentLabel, cacheChildren, insertion);
		}

		/// <summary>
		/// Executes the reduction as the reduction of a replaceable variable
		/// </summary>
		/// <param name="varIndex">The reduced variable index</param>
		/// <returns>The identifier of the produced SPPF node</returns>
		private int ReduceReplaceable(int varIndex)
		{
			int insertion = 0;
			for (int i = 0; i != handleNext; i++)
			{
				if (insertion != handleIndices[i])
					cacheChildren[insertion] = cacheChildren[handleIndices[i]];
				insertion++;
			}
			TableElemRef originalLabel = new TableElemRef(TableType.Variable, varIndex);
			return sppf.NewReplaceableNode(originalLabel, cacheChildren, handleActions, handleNext);
		}

		/// <summary>
		/// Adds the specified GSS label to the current history
		/// </summary>
		/// <param name="generation">The current generation</param>
		/// <param name="label">The label identifier of the SPPF node to use as a GSS label</param>
		private void AddToHistory(int generation, int label)
		{
			HistoryPart hp = GetHistoryPart(generation);
			if (hp == null)
			{
				hp = poolHPs.Acquire();
				hp.generation = generation;
				hp.next = 0;
				if (history.Length == nextHP)
					Array.Resize(ref history, history.Length + INIT_HISTORY_SIZE);
				history[nextHP++] = hp;
			}
			if (hp.next == hp.data.Length)
				Array.Resize(ref hp.data, hp.data.Length + INIT_HISTORY_PART_SIZE);
			hp.data[hp.next++] = label;
		}

		/// <summary>
		/// Finalizes the parse tree and returns it
		/// </summary>
		/// <param name="root">The identifier of the SPPF node that serves as root</param>
		/// <returns>The final parse tree</returns>
		public AST GetTree(int root)
		{
			AST.Node astRoot = BuildFinalAST(new SPPFNodeRef(root, 0));
			result.StoreRoot(astRoot);
			return result;
		}

		/// <summary>
		/// Builds the final AST for the specified SPPF node reference
		/// </summary>
		/// <param name="reference">A reference to an SPPF node in a specific version</param>
		/// <returns>The AST node for the SPPF reference</returns>
		public AST.Node BuildFinalAST(SPPFNodeRef reference)
		{
			SPPFNode sppfNode = sppf.GetNode(reference.NodeId);
			SPPFNodeVersion version = (sppfNode as SPPFNodeNormal).GetVersion(reference.Version);

			if (version.ChildrenCount == 0)
				return new AST.Node(version.Label);

			AST.Node[] buffer = new AST.Node[version.ChildrenCount];
			for (int i = 0; i != version.ChildrenCount; i++)
				buffer[i] = BuildFinalAST(version.Children[i]);
			int first = result.Store(buffer, 0, version.ChildrenCount);
			return new AST.Node(version.Label, version.ChildrenCount, first);
		}
	}
}