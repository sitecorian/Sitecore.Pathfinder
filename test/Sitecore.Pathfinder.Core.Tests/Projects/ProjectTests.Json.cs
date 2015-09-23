﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System.Linq;
using NUnit.Framework;
using Sitecore.Pathfinder.Projects.Items;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Projects
{
    [TestFixture]
    public partial class ProjectTests
    {
        /*
        [Test]
        public void JsonContentItemTest()
        {
            var projectItem = Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/JsonContentItem");
            Assert.IsNotNull(projectItem);
            Assert.AreEqual("JsonContentItem", projectItem.ShortName);
            Assert.AreEqual("/sitecore/content/Home/JsonContentItem", projectItem.QualifiedName);

            var item = projectItem as Item;
            Assert.IsNotNull(item);
            Assert.AreEqual("JsonContentItem", item.ItemName);
            Assert.AreEqual("/sitecore/content/Home/JsonContentItem", item.ItemIdOrPath);
            Assert.AreEqual("Sample Item", item.TemplateIdOrPath);
            Assert.IsNotNull(item.ItemNameProperty.SourceTextNodes);
            Assert.IsInstanceOf<FileNameTextNode>(item.ItemNameProperty.SourceTextNode);
            Assert.IsNotNull(item.TemplateIdOrPathProperty.SourceTextNodes);
            Assert.IsInstanceOf<AttributeNameTextNode>(item.TemplateIdOrPathProperty.SourceTextNode);
            Assert.AreEqual("Sample Item", TraceHelper.GetTextNode(item.TemplateIdOrPathProperty).Value);

            var field = item.Fields.FirstOrDefault(f => f.FieldName == "Text");
            Assert.IsNotNull(field);
            Assert.AreEqual("Hello World", field.Value);

            var textDocument = projectItem.Snapshots.First() as ITextSnapshot;
            Assert.IsNotNull(textDocument);
        }
        */
        [Test]
        public void JsonItemTest()
        {
            var projectItem = Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/JsonItem");
            Assert.IsNotNull(projectItem);
            Assert.AreEqual("JsonItem", projectItem.ShortName);
            Assert.AreEqual("/sitecore/content/Home/JsonItem", projectItem.QualifiedName);

            var item = projectItem as Item;
            Assert.IsNotNull(item);
            Assert.AreEqual("JsonItem", item.ItemName);
            Assert.AreEqual("/sitecore/content/Home/JsonItem", item.ItemIdOrPath);
            Assert.AreEqual("/sitecore/templates/Sample/JsonItem", item.TemplateIdOrPath);
            Assert.AreEqual(1, item.ItemNameProperty.SourceTextNodes.Count);
            Assert.IsInstanceOf<FileNameTextNode>(item.ItemNameProperty.SourceTextNode);

            var textDocument = projectItem.Snapshots.First() as ITextSnapshot;
            Assert.IsNotNull(textDocument);

            var treeNode = textDocument.Root;
            Assert.AreEqual("Item", treeNode.Name);
            Assert.AreEqual(5, treeNode.Attributes.Count());

            var attr = treeNode.Attributes.First();
            Assert.AreEqual("Template", attr.Name);
            Assert.AreEqual("/sitecore/templates/Sample/JsonItem", attr.Value);

            attr = treeNode.Attributes.ElementAt(1);
            Assert.AreEqual("Template.CreateFromFields", attr.Name);
            Assert.AreEqual("True", attr.Value);

            // text field
            var field = item.Fields.FirstOrDefault(f => f.FieldName == "Title");
            Assert.IsNotNull(field);
            Assert.AreEqual("Hello", field.Value);

            // link field
            var linkField = item.Fields.FirstOrDefault(f => f.FieldName == "Link");
            Assert.IsNotNull(linkField);
            Assert.AreEqual("/sitecore/media library/mushrooms", linkField.Value);
            Assert.AreEqual("<link text=\"\" linktype=\"internal\" url=\"\" anchor=\"\" title=\"\" class=\"\" target=\"\" querystring=\"\" id=\"{62A9DD2C-72FC-F9FF-B9B8-9FC477002D0D}\" />", linkField.ResolvedValue);

            // image field
            var imageField = item.Fields.FirstOrDefault(f => f.FieldName == "Image");
            Assert.IsNotNull(imageField);
            Assert.AreEqual("/sitecore/media library/mushrooms", imageField.Value);
            Assert.AreEqual("<image mediapath=\"\" alt=\"\" width=\"\" height=\"\" hspace=\"\" vspace=\"\" showineditor=\"\" usethumbnail=\"\" src=\"\" mediaid=\"{62A9DD2C-72FC-F9FF-B9B8-9FC477002D0D}\" />", imageField.ResolvedValue);

            // implicit link field
            var itemPathField = item.Fields.FirstOrDefault(f => f.FieldName == "ItemPath");
            Assert.IsNotNull(itemPathField);
            Assert.AreEqual("/sitecore/media library/mushrooms", itemPathField.Value);
            Assert.AreEqual("{62A9DD2C-72FC-F9FF-B9B8-9FC477002D0D}", itemPathField.ResolvedValue);

            // checkbox fields
            var checkBoxField = item.Fields.FirstOrDefault(f => f.FieldName == "TrueCheckbox");
            Assert.IsNotNull(checkBoxField);
            Assert.AreEqual("1", checkBoxField.ResolvedValue);
            checkBoxField = item.Fields.FirstOrDefault(f => f.FieldName == "FalseCheckbox");
            Assert.IsNotNull(checkBoxField);
            Assert.AreEqual(string.Empty, checkBoxField.ResolvedValue);

            // layout field
            var layout = item.Fields.FirstOrDefault(f => f.FieldName == "__Renderings");
            Assert.IsNotNull(layout);
            Assert.AreEqual(@"<r>
  <d id=""{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}"" l=""{1A5A92AD-D537-7E87-FB00-A39BFDE2538B}"">
    <r id=""{663E1E86-C959-7A70-8945-CFCEA79AFAC2}"" ds=""{11111111-1111-1111-1111-111111111111}"" par="""" ph=""Page.Body"" />
  </d>
</r>", layout.ResolvedValue);

            // unversioned field
            var unversionedFields = item.Fields.Where(f => f.FieldName == "UnversionedField").ToList();
            Assert.AreEqual(1, unversionedFields.Count);
            var unversionedField = unversionedFields.First();
            Assert.AreEqual("da-DK", unversionedField.Language);
            Assert.AreEqual(0, unversionedField.Version);

            // versioned field
            var versionedFields = item.Fields.Where(f => f.FieldName == "VersionedField").ToList();
            Assert.AreEqual(2, versionedFields.Count);
            var versionedField0 = versionedFields.First();
            Assert.AreEqual("Version 1", versionedField0.Value);
            Assert.AreEqual("da-DK", versionedField0.Language);
            Assert.AreEqual(1, versionedField0.Version);

            var versionedField1 = versionedFields.Last();
            Assert.AreEqual("Version 2", versionedField1.Value);
            Assert.AreEqual("da-DK", versionedField1.Language);
            Assert.AreEqual(2, versionedField1.Version);
        }

        [Test]
        public void JsonLayoutTest()
        {
            var projectItem = Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/JsonLayout");
            Assert.IsNotNull(projectItem);
            Assert.AreEqual("JsonLayout", projectItem.ShortName);
            Assert.AreEqual("/sitecore/content/Home/JsonLayout", projectItem.QualifiedName);

            var item = projectItem as Item;
            Assert.IsNotNull(item);

            var layout = item.Fields.FirstOrDefault(f => f.FieldName == "__Renderings");
            Assert.IsNotNull(layout);
            Assert.AreEqual(@"<r>
  <d id=""{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}"" l=""{1A5A92AD-D537-7E87-FB00-A39BFDE2538B}"">
    <r id=""{663E1E86-C959-7A70-8945-CFCEA79AFAC2}"" ds=""{11111111-1111-1111-1111-111111111111}"" par="""" ph=""Page.Body"" />
    <r id=""{663E1E86-C959-7A70-8945-CFCEA79AFAC2}"" par="""" ph=""Page.Body"" />
    <r id=""{663E1E86-C959-7A70-8945-CFCEA79AFAC2}"" par="""" ph=""Page.Body"" />
  </d>
</r>", layout.ResolvedValue);
        }
    }
}