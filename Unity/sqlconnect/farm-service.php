<?php
    // FOR DEBUG ONLY 
    session_start();
    $_SESSION['farm_id'] = 1;
    $_SESSION['user_id'] = 1;
    //

    $con = mysqli_connect('localhost', 'root', '', 'da_ai'); 

    //check connection to DB
    if (mysqli_connect_errno()){
        echo "connection failed";//error code: connection failed
        exit();
    }   

    $farm_id = $_SESSION['farm_id'];
    $user_id = $_SESSION['user_id'];

    $res = $con->query("SELECT * FROM farm 
                WHERE farm_id ='$farm_id' AND user_id = '$user_id'");

    $farm = mysqli_fetch_assoc($res);

    echo json_encode($farm);
?>