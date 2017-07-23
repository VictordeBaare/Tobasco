
<h3 align="center">Tobasco</h3>

<p align="center">Easy to use generator for your entities</p>


<h3>How do you use it?</h4>

<p>
Tobasco features a couple of default builders for a quick start.
These builder can be used to generate basic entity classes, database tables and CRUD stored procedures, repositories, Ninject module and mapping between generated entities.
</p>

<h3>T4</h3>
<p>
To use Tobasco to generate your classes, first add a T4 template. In the T4 template add the following lines of code:
</p>

<pre>
<#@ template debug="true" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@ assembly name="$(SolutionDir)LocationOffDll\Tobasco.dll" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ output extension=".cs" #>
<#@ import namespace="Tobasco" #>
<#@ import namespace="Tobasco.Manager" #>
<#@ import namespace="Tobasco.Enums" #>
<#
	var processor = FileProcessor.Create(this);    

	processor.BeginProcessing(Host.ResolvePath(string.Empty));
#>
</pre>

<p>
The Host.ResolvePath(string.Empty) is used to point to the location of your xmls. The xmls are the basis of your entity definition.
In the current example the xml files are in the same directory and folder as the generator.
</p>

<h3>Xml definition</h3>

<p>
For your entity definitions you'll have to start with an MainInfo.xml file. This file describes the basic values for the entities. 
The basic values include: 
<ul>
  <li>EntityLocations</li>
  <li>BaseNamespaces</li>
  <li>Mappers</li>
  <li>Database</li>
  <li>Repository</li>
  <li>DependencyInjection</li>
  <li>ConnectionFactory</li>
  <li>GenericRepository</li>
</ul>

<h4>EntityLocations</h4>
In the EntityLocations element you can add n locations in which you want to generate your entity.
For example:
</p>

<code>

```xml
<?xml version="1.0" encoding="utf-8" ?>
<EntityInformation>
  <EntityLocations>
        <EntityLocation>
            <FileLocation project="TobascoTest" folder="GeneratedEntity"></FileLocation>
        </EntityLocation>
        <EntityLocation generaterules="true">
            <FileLocation project="TobascoTest" folder="GeneratedEntity2"></FileLocation>
        </EntityLocation>
    </EntityLocations>
</EntityInformation>
```
</code>

<p>
Using the example above entities will be generated in the project: 'TobascoTest' in the folders: 'GeneratedEntity' and 'GeneratedEntity2'.
The second entry of 'EntityLocation' has also the following attribute 'generaterules="true"' this attribute will tell Tobasco to generate businessrules for this entity.

Additional elements which can be added to the 'EntityLocation' element are:
<ul>
  <li>Namespaces</li>
  <li>ORMapper</li>
</ul>

<h5>Example</h5>
<code>

```xml
<EntityLocation>
    <FileLocation project="TobascoTest" folder="GeneratedEntity"></FileLocation>
    <Namespaces>
        <Namespace value="System.Dynamic"></Namespace>
    </Namespaces>
    <ORMapper type="Dapper"></ORMapper>
</EntityLocation>
```
</code>
</p> 

<h4>BaseNamespaces</h4>
<p>
The namespaces which should be added to every generated file.
<h5>Example</h5>
<code>

```xml
<EntityInformation>
    <BaseNamespaces>
        <Namespace value="System.Collections.Generic"></Namespace>
    </BaseNamespaces>
</EntityInformation>
```
</code>
</p>


<h4>Mappers</h4>
<p>
More information to follow.
</p>

<h4>Database</h4>
<p>
More information to follow.
</p>

<h4>Repository</h4>
<p>
More information to follow.
</p>

<h4>DependencyInjection</h4>
<p>
More information to follow.
</p>

<h4>ConnectionFactory</h4>
<p>
More information to follow.
</p>

<h4>GenericRepository</h4>
<p>
More information to follow.
</p>
