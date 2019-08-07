<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreMaster.Master" CodeBehind="Help.aspx.vb" Inherits="sselStore.Help" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="placeholderlayout">

        <h1>Help Information</h1>
        <h2 class="block">FAQ</h2>
        <a href="#what do i do">I haven't used the store before. What do I do?</a>
        <br />
        <br />
        <a href="#items charged">Every item has a price listed. Am I being charged for everything
            I order?</a>
        <br />
        <br />
        <a href="#why not phil">Why can't I just get stuff from Phil anytime during the day?</a>
        <br />
        <br />
        <a href="#how do I pay">How do I pay for my order?</a>
        <br />
        <br />
        <a href="#when and where">When and where can I pick up my order?</a>
        <br />
        <br />
        <a href="#I cant find">I can't find what I'm looking for. Help, please?</a>
        <br />
        <br />
        <a href="#orderstatus">What does each different order status mean?</a>
        <br />
        <br />
        <a href="#How do I check">How do I check the status of my order?</a>
        <br />
        <br />
        <a href="#How do I cancel">How do I cancel an order?</a>
        <br />
        <br />
        <br />
        <br />
        <br />
        <div class="plainbox">
            <a id="what do i do"><span class="paragraph_header_font">I haven't used the store before. What do I do?</span></a>
            <br />
            <br />
            <!--Please read the <a href="http://ssel-sched.eecs.umich.edu/StoreInstr.pdf" target="_blank">
                Online StoreInstructions</a> and look through the rest of these help pages.-->
            When we design this web store, we try to imitate the online shopping experience provided by the big retail store like Amazon.com.  The features in this store are tailored for the lab environment - which are much simpler than the features provided by the online retail store.  As long as you had online shopping experience, you shouldn't have any difficulties on using this website.  Try it out yourself by clicking a few buttons - you will find out how easy it is to use this website.  If you really have difficulties on using this website, please contact the store manager.
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="items charged"><span class="paragraph_header_font">Every item has a price listed. Am I being charged for everything
            I order?</span></a>
            <br />
            <br />
            Right now, the following items are NOT being charged:<br />
            <ul>
                <li>Batteries</li>
                <li>Beakers</li>
                <li>Dipper Basket</li>
                <li>Graduated cylinders</li>
                <li>Magnetic stir bars</li>
                <li>Petri dishes</li>
                <li>Quartz vacuum wand tips</li>
                <li>Razor blades</li>
                <li>Thermometer</li>
            </ul>
            Make sure you specify your correct project account when you order these items. Your
        account will only be charged for those items NOT listed above. If upon checkout there
        are no accounts listed, see your professor and ask him/her to provide Lana Tyrrell
        with an account number (even if the items you receive are not charged to your account).
        Note: Items charged are subject to change.
        <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="how do I pay"><span class="paragraph_header_font">Why can't I just get stuff from Phil anytime during the day?</span></a>
            <br />
            <br />
            We're hoping that a regularly scheduled pickup time will make things easier and
            more predictable for both lab users and staff.
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="why not phil"><span class="paragraph_header_font">How do I pay for my order?</span></a>
            <br />
            When you place an order, on the <b>Step 1: Confirm and Payment</b> page there is a section
            for Account Information. Select
            your project account from the "Project" menu. The items you order will be charged
            to this account.
            <br />
            Note: some items are not charged to specific project accounts. See above, <a href="#items charged">Am I being charged for everything I order?</a>
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="when and where"><span class="paragraph_header_font">When and where can I pick up my order?</span></a>
            <br />
            Any order placed before 11 a.m. can be picked up the same day from 1:30-2:30 p.m.
            in room 1346. Phil will be in the room to handle orders. Orders placed after
            11 a.m. will be processed and ready for pickup the next business day.
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="I cant find"><span class="paragraph_header_font">I can't find what I'm looking for. Help, please?</span></a>
            <br />
            Try using the search function. If you still
            can't find the item, it may not be available on the web store. You can acquire these
            items in the usual non-online way.
            <!--(For a complete list of items available on the
            web store, click <a href="http://ssel-sched.eecs.umich.edu/mmSSEL/Html/current-web-enabled-items.htm"
                onclick="window.open('http://ssel-sched.eecs.umich.edu/mmSSEL/Html/current-web-enabled-items.htm','itemswindow','height=400,width=300,scrollbars=yes,resizable=yes,left=200,top=50'); return false">
                here</a>).-->
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="orderstatus"><span class="paragraph_header_font">What does each different order status mean?</span></a>
            <br />
            Open status - you just placed the order, and LNF staff had not prepared the order items for this order yet.<br />
            Fulfilled status - LNF staff had prepared your order items, and they are waiting in the store room.
            <br />
            Closed status - You had picked the order items from the store.
            <br />
            Cancelled status - You decide to cancel an order after it had been prepared by the LNF staff<br />
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="How do I check"><span class="paragraph_header_font">How do I check the status of my order?</span></a>
            <br />
            To check the status of an order, click on the My Orders tab menu. All the orders you have placed
            with us (both through the web and through other means) are listed. Choose the order status drop down list to view
            the order(s) with specific status.  Click the "View Detail" image button to view the detail of any order.  Please note
            that you can only edit or delete order(s) with status of "Open".
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
        <div class="plainbox">
            <a id="How do I cancel"><span class="paragraph_header_font">How do I cancel an order?</span></a>
            <br />
            You can only cancel an order if it's still on Open status.  An order is in Open status as long as SSEL staff has not prepared
            the item(s) for your order yet.  To cancel an Open status order, just go to My Orders page and click on the "X" delete button that 
            corresponds to the order you want to delete.  If you want to cancel an order that has been prepared by the staff, you have to contact the staff personally.
            <br />
            <a href="#faq" style="font-size: x-small">Back to top</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
