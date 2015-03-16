$(document).ready(function() {

    $('#Password').keyup(function () {
        $('#result').html(checkStrength($('#Password').val()))
	})	
	
	function checkStrength(password){
    
	//initial strength
    var strength = 0
	
    //if the password length is less than 8, return message.
    if (password.length < 8) { 
		$('#result').removeClass()
		$('#result').addClass('short')
		return 'Too short' 
	}
    
    //length is ok, lets continue.
	
	//if length is 8 characters or more, increase strength value
	if (password.length > 9) strength += 1
	
	//if password contains both lower and uppercase characters, increase strength value
	if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/))  strength += 1
	
	//if it has numbers and characters, increase strength value
	if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/))  strength += 1 
	
	//if it has one special character, increase strength value
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/))  strength += 1
	
	//if it has two special characters, increase strength value
    if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
	
	//now we have calculated strength value, we can return messages
	
	//if value is less than 2
	if (strength < 2 ) {
		$('#result').removeClass()
		$('#result').addClass('weak')
		$("#result").css("color", "#E66C2C");
		return 'Weak'			
	} else if (strength == 2 ) {
		$('#result').removeClass()
		$('#result').addClass('good')
        $("#result").css("color", "#2D98F3");
		return 'Good'		
	} else {
		$('#result').removeClass()
		$('#result').addClass('strong')
		$("#result").css("color", "Green");
		return 'Strong'
	}
}
});