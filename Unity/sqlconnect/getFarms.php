<?php
    include_once("connection.php");

    //check connection to DB
    if (mysqli_connect_errno()){
        echo "connection failed";//error code #1 = connection failed
        exit();
    }    


    $userId = $_POST["userId"];


    //Get list of farm_id (of a certain user_id)
    $sql = "SELECT farm_id FROM farms WHERE user_id = '" . $userId . "';";
    //$sql = "SELECT farm_id FROM farms WHERE user_id = 2;";

    //$result = $con->query($sql);
    $result = mysqli_query($con, $sql);
    if(mysqli_num_rows($result) > 0){
        $farmList = array();
        while($farm = $result->fetch_assoc()){
            $farmList[] = $farm;
        }
        echo json_encode($farmList);

        //$farmList = $result->fetch_assoc();
        //echo count($farmList);
        
    }
    else{
        echo mysqli_num_rows($result);
    }
    

?>