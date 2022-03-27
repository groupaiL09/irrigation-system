<?php
    $con = mysqli_connect('localhost', 'root', '', 'da_ai'); 

    //check connection to DB
    if (mysqli_connect_errno()){
        echo "connection failed";//error code: connection failed
        exit();
    }    

    $farm_id = $_SESSION['farm_id'];
    $user_id = $_SESSION['user_id'];
    $name = $_POST['name'];
    $address = $_POST['address'];
    $area = $_POST['area'];

    $con->query("UPDATE farm 
                SET name = '$name', address = '$address', area = '$area' 
                WHERE farm_id ='$farm_id' AND user_id = '$user_id'");
    
    $con->close();
?>