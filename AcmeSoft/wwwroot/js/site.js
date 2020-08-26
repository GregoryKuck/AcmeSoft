var view = new Vue({
    el: '#main',
    data: {
        people: { data: [] },
        defaultEmployee: { employeeId: 0, personId: 0, employeeNum: '', employedDate: null, terminatedDate: null },
        employee: { employeeId: 0, personId: 0, employeeNum: '', employedDate: null, terminatedDate: null },
        selectedPerson: { personId: null } 
    },
    computed: {

    },
    methods: {
        addEmployeeToPerson(person) {
            view.selectedPerson.personId = person.personId;
            $("#employeeEditContainer").show();
            console.log(view.selectedPerson.personId);
        },
        editEmployee(employee) {
            console.log(employee);
            //HACK: This can be handled better
            view.selectedPerson.personId = employee.personId;
            view.employee = employee;
            $("#employeeEditContainer").show();
        },
        deleteEmployee(employee) {
            //HACK - Not DRY
            $.ajax({
                method: 'DELETE',
                url: `/v1/employee/${employee.employeeId}`,
                dataType: 'json',
                contentType: 'application/json',
                data: null,
                complete: function (resp) {
                    view.getAllPeopleAndEmployees();
                }
            });
        },
        addUpdateEmployee: function () {
            var employee = view.employee;
            employee.personId = view.selectedPerson.personId;

            //HACK: Not DRY
            $.ajax({
                method: 'POST',
                url: '/v1/employee',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(employee),
                complete: function (resp) {
                    //TODO: Move to function
                    view.getAllPeopleAndEmployees();
                    view.hideCard();
                }
            });
        },
        getAllPeopleAndEmployees: function () {
            this.serverCall('GET', "/v1/employee/people", null, view.people)
        },
        hideCard: function () {
            view.employee = view.defaultEmployee;
            view.selectedPerson.personId = null;
            $("#employeeEditContainer").hide();
        },
        serverCall: function (method, destUrl, obj, dataItem) {
            //HACK: this gave some issues but not debugging it for now, so just did some non-DRY stuff above for delete and upsert
            var jsonData = obj ? JSON.stringify(obj) : '';

            $.ajax({
                method: method,
                url: destUrl,
                dataType: "json",
                contentType: "application/json",
                data: jsonData,
                success: function (resp) {
                    if (dataItem !== null)
                        dataItem.data = resp.slice();
                }
            });
        }
    }
});

$(document).ready(function () {
    view.getAllPeopleAndEmployees();
});