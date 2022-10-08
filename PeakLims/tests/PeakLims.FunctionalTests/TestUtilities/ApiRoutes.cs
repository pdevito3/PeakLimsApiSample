namespace PeakLims.FunctionalTests.TestUtilities;
public class ApiRoutes
{
    public const string Base = "api";
    public const string Health = Base + "/health";

    // new api route marker - do not delete

    public static class HealthcareOrganizationContacts
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/healthcareOrganizationContacts";
        public const string GetRecord = $"{Base}/healthcareOrganizationContacts/{Id}";
        public const string Create = $"{Base}/healthcareOrganizationContacts";
        public const string Delete = $"{Base}/healthcareOrganizationContacts/{Id}";
        public const string Put = $"{Base}/healthcareOrganizationContacts/{Id}";
        public const string CreateBatch = $"{Base}/healthcareOrganizationContacts/batch";
    }

    public static class HealthcareOrganizations
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/healthcareOrganizations";
        public const string GetRecord = $"{Base}/healthcareOrganizations/{Id}";
        public const string Create = $"{Base}/healthcareOrganizations";
        public const string Delete = $"{Base}/healthcareOrganizations/{Id}";
        public const string Put = $"{Base}/healthcareOrganizations/{Id}";
        public const string CreateBatch = $"{Base}/healthcareOrganizations/batch";
    }

    public static class Tests
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/tests";
        public const string GetRecord = $"{Base}/tests/{Id}";
        public const string Create = $"{Base}/tests";
        public const string Delete = $"{Base}/tests/{Id}";
        public const string Put = $"{Base}/tests/{Id}";
        public const string CreateBatch = $"{Base}/tests/batch";
    }

    public static class Panels
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/panels";
        public const string GetRecord = $"{Base}/panels/{Id}";
        public const string Create = $"{Base}/panels";
        public const string Delete = $"{Base}/panels/{Id}";
        public const string Put = $"{Base}/panels/{Id}";
        public const string CreateBatch = $"{Base}/panels/batch";
    }

    public static class TestOrders
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/testOrders";
        public const string GetRecord = $"{Base}/testOrders/{Id}";
        public const string Create = $"{Base}/testOrders";
        public const string Delete = $"{Base}/testOrders/{Id}";
        public const string Put = $"{Base}/testOrders/{Id}";
        public const string CreateBatch = $"{Base}/testOrders/batch";
    }

    public static class PanelOrders
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/panelOrders";
        public const string GetRecord = $"{Base}/panelOrders/{Id}";
        public const string Create = $"{Base}/panelOrders";
        public const string Delete = $"{Base}/panelOrders/{Id}";
        public const string Put = $"{Base}/panelOrders/{Id}";
        public const string CreateBatch = $"{Base}/panelOrders/batch";
    }

    public static class Containers
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/containers";
        public const string GetRecord = $"{Base}/containers/{Id}";
        public const string Create = $"{Base}/containers";
        public const string Delete = $"{Base}/containers/{Id}";
        public const string Put = $"{Base}/containers/{Id}";
        public const string CreateBatch = $"{Base}/containers/batch";
    }

    public static class Samples
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/samples";
        public const string GetRecord = $"{Base}/samples/{Id}";
        public const string Create = $"{Base}/samples";
        public const string Delete = $"{Base}/samples/{Id}";
        public const string Put = $"{Base}/samples/{Id}";
        public const string CreateBatch = $"{Base}/samples/batch";
    }

    public static class AccessionComments
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/accessionComments";
        public const string GetRecord = $"{Base}/accessionComments/{Id}";
        public const string Create = $"{Base}/accessionComments";
        public const string Delete = $"{Base}/accessionComments/{Id}";
        public const string Put = $"{Base}/accessionComments/{Id}";
        public const string CreateBatch = $"{Base}/accessionComments/batch";
    }

    public static class Accessions
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/accessions";
        public const string GetRecord = $"{Base}/accessions/{Id}";
        public const string Create = $"{Base}/accessions";
        public const string Delete = $"{Base}/accessions/{Id}";
        public const string Put = $"{Base}/accessions/{Id}";
        public const string CreateBatch = $"{Base}/accessions/batch";
    }

    public static class Patients
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/patients";
        public const string GetRecord = $"{Base}/patients/{Id}";
        public const string Create = $"{Base}/patients";
        public const string Delete = $"{Base}/patients/{Id}";
        public const string Put = $"{Base}/patients/{Id}";
        public const string CreateBatch = $"{Base}/patients/batch";
    }

    public static class Users
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/users";
        public const string GetRecord = $"{Base}/users/{Id}";
        public const string Create = $"{Base}/users";
        public const string Delete = $"{Base}/users/{Id}";
        public const string Put = $"{Base}/users/{Id}";
        public const string CreateBatch = $"{Base}/users/batch";
        public const string AddRole = $"{Base}/users/{Id}/addRole";
        public const string RemoveRole = $"{Base}/users/{Id}/removeRole";
    }

    public static class RolePermissions
    {
        public const string Id = "{id}";
        public const string GetList = $"{Base}/rolePermissions";
        public const string GetRecord = $"{Base}/rolePermissions/{Id}";
        public const string Create = $"{Base}/rolePermissions";
        public const string Delete = $"{Base}/rolePermissions/{Id}";
        public const string Put = $"{Base}/rolePermissions/{Id}";
        public const string CreateBatch = $"{Base}/rolePermissions/batch";
    }
}
