// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EstateManagement.IntegrationTests.Estate
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "base")]
    [Xunit.TraitAttribute("Category", "shared")]
    public partial class EstateFeature : Xunit.IClassFixture<EstateFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "base",
                "shared"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Estate.feature"
#line hidden
        
        public EstateFeature(EstateFeature.FixtureData fixtureData, EstateManagement_IntegrationTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Estate", null, ProgrammingLanguage.CSharp, new string[] {
                        "base",
                        "shared"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "RoleName"});
            table1.AddRow(new string[] {
                        "Estate"});
#line 5
 testRunner.Given("the following security roles exist", ((string)(null)), table1, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "ResourceName",
                        "DisplayName",
                        "Secret",
                        "Scopes",
                        "UserClaims"});
            table2.AddRow(new string[] {
                        "estateManagement",
                        "Estate Managememt REST",
                        "Secret1",
                        "estateManagement",
                        "MerchantId, EstateId, role"});
#line 9
 testRunner.Given("the following api resources exist", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId",
                        "ClientName",
                        "Secret",
                        "AllowedScopes",
                        "AllowedGrantTypes"});
            table3.AddRow(new string[] {
                        "serviceClient",
                        "Service Client",
                        "Secret1",
                        "estateManagement",
                        "client_credentials"});
            table3.AddRow(new string[] {
                        "estateClient",
                        "Estate Client",
                        "Secret1",
                        "estateManagement",
                        "password"});
#line 13
 testRunner.Given("the following clients exist", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "ClientId"});
            table4.AddRow(new string[] {
                        "serviceClient"});
#line 18
 testRunner.Given("I have a token to access the estate management resource", ((string)(null)), table4, "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Create Estate")]
        [Xunit.TraitAttribute("FeatureTitle", "Estate")]
        [Xunit.TraitAttribute("Description", "Create Estate")]
        public virtual void CreateEstate()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Estate", null, ((string[])(null)));
#line 22
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName"});
                table5.AddRow(new string[] {
                            "Test Estate 1"});
#line 24
 testRunner.When("I create the following estates", ((string)(null)), table5, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Create Operator")]
        [Xunit.TraitAttribute("FeatureTitle", "Estate")]
        [Xunit.TraitAttribute("Description", "Create Operator")]
        public virtual void CreateOperator()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Operator", null, ((string[])(null)));
#line 28
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName"});
                table6.AddRow(new string[] {
                            "Test Estate 1"});
#line 29
 testRunner.Given("I have created the following estates", ((string)(null)), table6, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table7.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "True",
                            "True"});
#line 33
 testRunner.When("I create the following operators", ((string)(null)), table7, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Create Security User")]
        [Xunit.TraitAttribute("FeatureTitle", "Estate")]
        [Xunit.TraitAttribute("Description", "Create Security User")]
        public virtual void CreateSecurityUser()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Security User", null, ((string[])(null)));
#line 37
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName"});
                table8.AddRow(new string[] {
                            "Test Estate 1"});
#line 38
 testRunner.Given("I have created the following estates", ((string)(null)), table8, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                            "EmailAddress",
                            "Password",
                            "GivenName",
                            "FamilyName",
                            "EstateName"});
                table9.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User1",
                            "Test Estate 1"});
#line 41
 testRunner.When("I create the following security users", ((string)(null)), table9, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Estate - System Login")]
        [Xunit.TraitAttribute("FeatureTitle", "Estate")]
        [Xunit.TraitAttribute("Description", "Get Estate - System Login")]
        [Xunit.TraitAttribute("Category", "PRTest")]
        public virtual void GetEstate_SystemLogin()
        {
            string[] tagsOfScenario = new string[] {
                    "PRTest"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Estate - System Login", null, new string[] {
                        "PRTest"});
#line 47
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName"});
                table10.AddRow(new string[] {
                            "Test Estate 1"});
#line 48
 testRunner.Given("I have created the following estates", ((string)(null)), table10, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table11.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "True",
                            "True"});
#line 51
 testRunner.And("I have created the following operators", ((string)(null)), table11, "And ");
#line hidden
                TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                            "EmailAddress",
                            "Password",
                            "GivenName",
                            "FamilyName",
                            "EstateName"});
                table12.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User1",
                            "Test Estate 1"});
#line 54
 testRunner.And("I have created the following security users", ((string)(null)), table12, "And ");
#line hidden
                TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "EmailAddress",
                            "GivenName",
                            "FamilyName"});
                table13.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "estateuser1@testestate1.co.uk",
                            "TestEstate",
                            "User1"});
#line 57
 testRunner.When("I get the estate \"Test Estate 1\" the estate details are returned as follows", ((string)(null)), table13, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Get Estate - Estate user")]
        [Xunit.TraitAttribute("FeatureTitle", "Estate")]
        [Xunit.TraitAttribute("Description", "Get Estate - Estate user")]
        [Xunit.TraitAttribute("Category", "ignore")]
        [Xunit.TraitAttribute("Category", "-")]
        [Xunit.TraitAttribute("Category", "need")]
        [Xunit.TraitAttribute("Category", "to")]
        [Xunit.TraitAttribute("Category", "build")]
        [Xunit.TraitAttribute("Category", "in")]
        [Xunit.TraitAttribute("Category", "a")]
        [Xunit.TraitAttribute("Category", "subscription")]
        [Xunit.TraitAttribute("Category", "service")]
        [Xunit.TraitAttribute("Category", "PRTest")]
        public virtual void GetEstate_EstateUser()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore",
                    "-",
                    "need",
                    "to",
                    "build",
                    "in",
                    "a",
                    "subscription",
                    "service",
                    "PRTest"};
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Estate - Estate user", null, new string[] {
                        "ignore",
                        "-",
                        "need",
                        "to",
                        "build",
                        "in",
                        "a",
                        "subscription",
                        "service",
                        "PRTest"});
#line 63
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 4
this.FeatureBackground();
#line hidden
                TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName"});
                table14.AddRow(new string[] {
                            "Test Estate 1"});
#line 64
 testRunner.Given("I have created the following estates", ((string)(null)), table14, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "RequireCustomMerchantNumber",
                            "RequireCustomTerminalNumber"});
                table15.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "True",
                            "True"});
#line 67
 testRunner.And("I have created the following operators", ((string)(null)), table15, "And ");
#line hidden
                TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                            "EmailAddress",
                            "Password",
                            "GivenName",
                            "FamilyName",
                            "EstateName"});
                table16.AddRow(new string[] {
                            "estateuser1@testestate1.co.uk",
                            "123456",
                            "TestEstate",
                            "User1",
                            "Test Estate 1"});
#line 70
 testRunner.And("I have created the following security users", ((string)(null)), table16, "And ");
#line hidden
#line 73
 testRunner.And("I am logged in as \"estateuser1@testestate1.co.uk\" with password \"123456\" for Esta" +
                        "te \"Test Estate 1\" with client \"estateClient\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                            "EstateName",
                            "OperatorName",
                            "EmailAddress",
                            "GivenName",
                            "FamilyName"});
                table17.AddRow(new string[] {
                            "Test Estate 1",
                            "Test Operator 1",
                            "estateuser1@testestate1.co.uk",
                            "TestEstate",
                            "User1"});
#line 74
 testRunner.When("I get the estate \"Test Estate 1\" the estate details are returned as follows", ((string)(null)), table17, "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                EstateFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                EstateFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
