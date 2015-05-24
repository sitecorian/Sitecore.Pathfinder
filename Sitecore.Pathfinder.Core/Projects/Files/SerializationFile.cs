﻿namespace Sitecore.Pathfinder.Projects.Files
{
  using System.Diagnostics;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.Documents;
  using Sitecore.Pathfinder.Projects.Items;

  public class SerializationFile : File
  {
    public SerializationFile([NotNull] IProject project, [NotNull] IDocumentSnapshot documentSnapshot, [NotNull] Item item) : base(project, documentSnapshot)
    {
      this.Item = item;

      Debug.Assert(this.Item.Owner != null, "Owner is already set");
      this.Item.Owner = this;
    }

    [NotNull]
    public Item Item { get; }
  }
}
