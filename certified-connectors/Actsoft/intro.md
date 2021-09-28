**Connector Description:**

Actsoft Mobile Workforce+ allows you to create and capture digital form
information, employee timekeeping information and manage work orders with a
dispatching system.

**Detailed Description**

The Actsoft Mobile Workforce+ connector enables users to connect and exchange
information between multiple platforms. You can animate your data to create
process flows, drive your other in-house applications, and inform powerful
analytics. Automate and streamline your daily, weekly or monthly processes with
access to up-to-the-moment actionable data.

**\#\# Prerequisites**

To use this connector, you will need the following:

-   Active Mobile Workforce+ account

-   Mobile Workforce+ API key

-   Microsoft Power Apps or Power Automate plan

**\#\# How to get credentials**

**API Developer portal**

To access the connector, you must first setup a username and password for the
API Developer portal and subscribe to the Customer API:

1.  From inside the application, go to **Administrative** \> **Settings** \>
    **API** **Setup**.

2.  Choose **Create Credentials**.

3.  Enter your first name and last name in the appropriate fields.

4.  Enter your email address in the **Email** **address** field.

5.  Choose **Save & email invitation**. Then, check your email for instructions
    on creating a password.

6.  Once inside the invitation email, click on **Choose** **Password**, which
    will open a dialog that will allow you to create a password.

7.  Choose **Save password**.

8.  To proceed to the developer portal, from inside the application, go to
    **Administrative** \> **Settings** \> **Go** **to the developer portal**.

9.  Inside the Developer portal, choose **Products**, and then select
    **Customer** **API**.

10. Choose **Subscribe**.

**API Key**

Now that you are a registered API user, an alphanumeric sequence, called a key,
has been generated for you. You will need this key to work with your data in the
API.

**To locate your API Key:**

1.  Choose **Products** from the Developer portal menu. The API products
    available in your license will display.

2.  Select the **Customer API** version, under ‘You have X subscription(s) to
    this product’.

3.  Choose **Show** next to the Primary key for your subscription.

4.  Select the contents of the field, right-click and choose **Copy**. The key
    is now saved to your clipboard, and ready to paste.

**\#\# Get started with your connector**

1.  Go to **My flows** \> **New flow**, and then select **Automated cloud
    flow**.

2.  Choose **Manually trigger a flow** from the Build an automated cloud flow
    dialog.

3.  Name your flow, or let the system generate one automatically.

4.  Search for and choose a trigger for your flow.

5.  Choose **Create**.

6.  In the search field, search for and choose the Actsoft Mobile Workforce+
    connector.

7.  Find the action that you want to perform with the connector.

The system will prompt you to login to all apps required to perform your chosen
action, skipping any that you are already signed into.

1.  Enter a name for your new connector in the Connector field.

2.  Then, enter your API key.

3.  Choose **Create**. The first step in your process flow displays as the first
    ‘box’ in a flowchart.

4.  Choose **New** **step** to continue adding more actions and steps to your
    connector process flow.

5.  Choose **Save**.

\*\*Using the connector - Order status poll example  
\*\* The instructions below show you how to create a process flow to insert rows
in a worksheet, and insert identifying data of the employee who updated the
order when an order status is updated.

1.  Go to **Connectors**.

2.  Search for and choose the [Actsoft Mobile Workforce+] connector.

3.  Choose *Order statuses poll* from the actions listing.

4.  Enter a name for your new connection in the *Connection name* field.

5.  Then, enter your API key.

6.  Choose **Create**. The first step in your process flow displays as the first
    ‘box’ in a flowchart.

Each action dialog in the steps of your connection flow will have property
fields that you can customize to control the action performed.

1.  Continuing with the example, choose whether you want Forms field values to
    display (*excludeFormData parameter*).

2.  Choose the X-API version for the connection.

3.  Choose **Next step** to set the action you would like to occur when an order
    status is changed.

4.  In the **Choose an operation** dialog, use the search field to locate the
    application and then the desired action. For example, if you wanted to
    insert a row in a Google worksheet each time an order status is changed, you
    would choose **Google** **Sheets** and then **Insert** **row** for the
    action.

5.  In the **Insert** **row** dialog that follows, click in the **File** field,
    and choose the file that will contain your results.

6.  In the **Worksheet** field, choose the *data* worksheet type. The fields
    that dynamically display are the data columns in the worksheet.

7.  Click in the field for each column, and choose a property from the *Order
    status poll* properties listing. Or, you can use the search field to find a
    specific property.

8.  You may enter more than one property in each column field; in the Event
    column, choose StatusLabel, insert a dash, and then choose StatusStartDate.
    This means that for each order status update, the Status Label and the
    Status Start Date will write to the Event column in the new row.

9.  Select **New** **step**. As an aside note, you can always hover and click
    the arrow connector to insert a new step

10. In the Choose an operation dialog, select the Mobile Workforce+ connector
    again and then **Create a client** for the action.

Again, each field displayed in the dialog will represent a column in the Google
Sheets spreadsheet. Click in each field to activate the *Order status poll*
properties listing, and select the properties you want.

1.  For this example, choose *EmployeeNumber* for the **Name** field, to
    identify the employee who updated the order status.

2.  Choose *GroupName* and insert it also into the **Name** field.

3.  Choose **Save**.

Now, you have created a process flow that will update your spreadsheet with the
employee number and their associated group each time an order status is updated.

**\#\# Known issues and limitations**

-   Audio file binary uploads (POST) for all endpoints are limited to MP4 only.

-   Dates for all endpoints that require date-time information must be specified
    according to RFC3339 guidelines, as in the following example:
    2021-09-22T02:36:56.52Z. It is not necessary to specify the exact seconds in
    the timestamp; 00 is acceptable.

**\#\# Common errors and remedies**

Examples of common errors that may occur, response codes and corrections can be
found at the API Developers Portal(s):

Encore - <https://developer.wfmplatform.com/>

**\#\# FAQs**
