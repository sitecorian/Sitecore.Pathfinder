﻿namespace Sitecore.Pathfinder.Projects
{
  using System.IO;
  using System.Linq;
  using NUnit.Framework;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.Documents;
  using Sitecore.Pathfinder.Projects.Items;
  using Sitecore.Pathfinder.Projects.Templates;

  [TestFixture]
  public class ProjectTests : Tests
  {
    [NotNull]
    public IProject Project { get; set; }

    [TestFixtureSetUp]
    public void Startup()
    {
      this.Start();
      this.Project = this.Services.ProjectService.LoadProjectFromConfiguration();
    }

    [Test]
    public void RemapFileDirectoryTests()
    {
      Assert.AreEqual(1, this.Project.Options.RemapFileDirectories.Count);
      Assert.AreEqual("/sitecore/shell/client", this.Project.Options.RemapFileDirectories["/sitecore/client"]);
    }

    [Test]
    public void ExternalReferencesTests()
    {
      Assert.AreEqual(8, this.Project.Options.ExternalReferences.Count);
      Assert.AreEqual("/sitecore/templates/Sample/Sample Item", this.Project.Options.ExternalReferences.ElementAt(0));
    }

    [Test]
    public void AddRemoveTests()
    {
      var project = this.Resolve<IProject>().Load(new ProjectOptions(this.ProjectDirectory, "master"), Enumerable.Empty<string>());

      var fileName = Path.Combine(this.ProjectDirectory, "content\\Home\\HelloWorld.item.xml");

      project.Add(fileName);
      Assert.AreEqual(1, project.SourceFiles.Count);
      Assert.AreEqual(fileName, project.SourceFiles.First().FileName);

      project.Remove(fileName);
      Assert.AreEqual(0, project.SourceFiles.Count);
    }

    [Test]
    public void JsonItemTest()
    {
      var projectItem = this.Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/Foo");
      Assert.IsNotNull(projectItem);
      Assert.AreEqual("Foo", projectItem.ShortName);
      Assert.AreEqual("/sitecore/content/Home/Foo", projectItem.QualifiedName);

      var item = projectItem as Item;
      Assert.IsNotNull(item);
      Assert.AreEqual("Foo", item.ItemName);
      Assert.AreEqual("/sitecore/content/Home/Foo", item.ItemIdOrPath);
      Assert.AreEqual("/sitecore/templates/Sample/HelloWorld", item.TemplateIdOrPath);

      var textDocument = projectItem.Snapshot as ITextSnapshot;
      Assert.IsNotNull(textDocument);

      var treeNode = textDocument.Root;
      Assert.AreEqual("Item", treeNode.Name);
      Assert.AreEqual(1, treeNode.Attributes.Count);

      var attr = treeNode.Attributes.First();
      Assert.AreEqual("Template.Create", attr.Name);
      Assert.AreEqual("/sitecore/templates/Sample/HelloWorld", attr.Value);

      var field = item.Fields.FirstOrDefault(f => f.FieldName == "Title");
      Assert.IsNotNull(field);
      Assert.AreEqual("Hello", field.Value);
    }

    [Test]
    public void SerializationItemTest()
    {
      var projectItem = this.Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/Pipper");
      Assert.IsNotNull(projectItem);
      Assert.AreEqual("Pipper", projectItem.ShortName);
      Assert.AreEqual("/sitecore/content/Home/Pipper", projectItem.QualifiedName);

      var item = projectItem as Item;
      Assert.IsNotNull(item);
      Assert.AreEqual("Pipper", item.ItemName);
      Assert.AreEqual("/sitecore/content/Home/Pipper", item.ItemIdOrPath);
      Assert.AreEqual("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}", item.TemplateIdOrPath);

      var field = item.Fields.FirstOrDefault();
      Assert.IsNotNull(field);
      Assert.AreEqual("__Workflow", field.FieldName);
      Assert.AreEqual("{A5BC37E7-ED96-4C1E-8590-A26E64DB55EA}", field.Value);

      field = item.Fields.ElementAt(1);
      Assert.IsNotNull(field);
      Assert.AreEqual("Title", field.FieldName);
      Assert.AreEqual("Pip 1", field.Value);
      Assert.AreEqual("en", field.Language);
      Assert.AreEqual(1, field.Version);
    }

    [Test]
    public void JsonTemplateTest()
    {
      var projectItem = this.Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/FooTemplate");
      Assert.IsNotNull(projectItem);
      Assert.AreEqual("FooTemplate", projectItem.ShortName);
      Assert.AreEqual("/sitecore/content/Home/FooTemplate", projectItem.QualifiedName);

      var template = projectItem as Template;
      Assert.IsNotNull(template);
      Assert.AreEqual("FooTemplate", template.ItemName);
      Assert.AreEqual("Applications/16x16/About.png", template.Icon);
      Assert.AreEqual("ShortHelp", template.ShortHelp);
      Assert.AreEqual("LongHelp", template.LongHelp);
      Assert.AreEqual("/sitecore/content/Home/FooTemplate", template.ItemIdOrPath);
      Assert.AreEqual(1, template.Sections.Count);

      var section = template.Sections[0];
      Assert.AreEqual("Fields", section.Name);
      Assert.AreEqual("Applications/16x16/About.png", section.Icon);
      Assert.AreEqual(2, section.Fields.Count);

      var field = section.Fields[0];
      Assert.AreEqual("Title", field.Name);
      Assert.AreEqual("Single-Line Text", field.Type);
      Assert.AreEqual("ShortHelp", field.ShortHelp);
      Assert.AreEqual("LongHelp", field.LongHelp);
      Assert.AreEqual("StandardValue", field.StandardValue);

      Assert.AreEqual("Rich Text", section.Fields[1].Type);
    }

    [Test]
    public void LoadProjectTests()
    {
      Assert.IsTrue(this.Project.Items.Any());
      Assert.IsTrue(this.Project.SourceFiles.Any());
    }

    [Test]
    public void XmlItemTest()
    {
      var projectItem = this.Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/HelloWorld");
      Assert.IsNotNull(projectItem);
      Assert.AreEqual("HelloWorld", projectItem.ShortName);
      Assert.AreEqual("/sitecore/content/Home/HelloWorld", projectItem.QualifiedName);

      var item = projectItem as Item;
      Assert.IsNotNull(item);
      Assert.AreEqual("HelloWorld", item.ItemName);
      Assert.AreEqual("/sitecore/content/Home/HelloWorld", item.ItemIdOrPath);
      Assert.AreEqual("/sitecore/templates/Sample/HelloWorld", item.TemplateIdOrPath);

      var field = item.Fields.FirstOrDefault(f => f.FieldName == "Title");
      Assert.IsNotNull(field);
      Assert.AreEqual("Hello", field.Value);

      var textDocument = projectItem.Snapshot as ITextSnapshot;
      Assert.IsNotNull(textDocument);

      var treeNode = textDocument.Root;
      Assert.AreEqual("Item", treeNode.Name);
      Assert.AreEqual(1, treeNode.Attributes.Count);

      var attr = treeNode.Attributes.First();
      Assert.AreEqual("Template.Create", attr.Name);
      Assert.AreEqual("/sitecore/templates/Sample/HelloWorld", attr.Value);
    }

    [Test]
    public void MergeTest()
    {
      var projectItem = this.Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/media library/Mushrooms");
      Assert.IsNotNull(projectItem);
      Assert.AreEqual("Mushrooms", projectItem.ShortName);
      Assert.AreEqual("/sitecore/media library/Mushrooms", projectItem.QualifiedName);

      var item = projectItem as Item;
      Assert.IsNotNull(item);
      Assert.AreEqual("Mushrooms", item.ItemName);
      Assert.AreEqual("/sitecore/media library/Mushrooms", item.ItemIdOrPath);

      var field = item.Fields.FirstOrDefault(f => f.FieldName == "Description");
      Assert.IsNotNull(field);
      Assert.AreEqual("Mushrooms", field.Value);
    }

    [Test]
    public void MergeByProjectUniqueIdTest()
    {
      var project = this.Resolve<IProject>();

      var projectItem1 = new Item(project, "SameId", Snapshot.Empty);
      var projectItem2 = new Item(project, "SameId", Snapshot.Empty);

      project.AddOrMerge(projectItem1);
      project.AddOrMerge(projectItem2);

      Assert.AreEqual(1, project.Items.Count());
    }
  }
}
